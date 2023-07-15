using Microsoft.AspNetCore.Mvc;
using Tasker.Models;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
using Npgsql;
using DotNetEnv;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskFolderController : ControllerBase
    {
        public static List<TaskFolder> Folders = new List<TaskFolder>()
        {
            new TaskFolder{OwnerId = 1, OwnerName="John Doe", OwnerEmail= "JDoe@albany.edu" }, // add ()
            new TaskFolder{OwnerId = 2, OwnerName="Jane Doe", OwnerEmail= "JDoe2@albany.edu" },
            new TaskFolder{OwnerId = 3, OwnerName="Alice Brown", OwnerEmail= "ABrown@albany.edu" },
            new TaskFolder{OwnerId = 4, OwnerName="Bob Green", OwnerEmail= "BGreen@albany.edu" },
            new TaskFolder{OwnerId = 5, OwnerName="Nancy Yellow", OwnerEmail= "NYellow@albany.edu" }

        };

        //[HttpGet("{employeeId:int}")]
        [HttpGet]
        public async Task<List<TaskFolder>> GetAsync(int employeeId, int relation)
        //public async Task<ListObjectsResponse> GetAsync(int employeeId)
        {


            ////////////////////////
            ///
            //Console.WriteLine(employeeId + " " + relation);
            //return Folders;
            //////////////////////////////


            
            List<TaskFolder> folders = new List<TaskFolder>();

            var fgaClient = CreateStoreClient("01H1AM5QQYN9VZTJ8MNW2HXAJV");

            ListObjectsResponse response = new ListObjectsResponse();

            if (relation == 1)
                response = await ListCheck(fgaClient, "01H5B0VND3034JA8BJP4GBMWH7", "employee:" + employeeId, "supervisor_plus", "employee");
            else if(relation == 2)
                response = await ListCheck(fgaClient, "01H5B0VND3034JA8BJP4GBMWH7", "employee:" + employeeId, "assistant", "employee");

            if (response.Objects != null)
            {
                for (int i = 0; i < response.Objects.Count; i++)
                {
                    string? employee = response.Objects[i];

                    string ID = employee.Split(':')[1];

                    folders.Add(GetTaskFolder(ID));

                }

                /*foreach (var folder in folders)
                {
                    Console.WriteLine(folder.OwnerId + " " + folder.OwnerName + " " + folder.OwnerEmail);
                }*/

            }
            return folders;

        }


        public static TaskFolder GetTaskFolder(String employeeID)
        {
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            string query;

            using (NpgsqlConnection con = GetConnection())
            {
                query = $"SELECT employee_id, first_name, last_name, email FROM employee" +
                    $" WHERE employee_id = {employeeID};";
                command = new NpgsqlCommand(@query, con);
                con.Open();
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read()) //while to read multiple
                    {
                        //read the data and create a TaskFolder object
                        string FirstName = (string)reader["first_name"];
                        string LastName = (string)reader["last_name"];
                        string Email = (string)reader["email"];

                        reader.Close();
                        con.Close();

                        return new TaskFolder { OwnerId = int.Parse(employeeID), OwnerName = FirstName + " " + LastName, OwnerEmail = Email };
                    }

                }
                return null;
            }

        }


        private static NpgsqlConnection GetConnection()
        {
            Env.TraversePath().Load();
            string? DB_HOST = Environment.GetEnvironmentVariable("HOST");
            string? DB_PORT = Environment.GetEnvironmentVariable("PORT");
            string? DB_NAME = Environment.GetEnvironmentVariable("DATABASE");
            string? DB_USERNAME = Environment.GetEnvironmentVariable("USERNAME");
            string? DB_PASSWORD = Environment.GetEnvironmentVariable("PASSWORD");
            string connectionString = "Server=" + DB_HOST + ";Port=" + DB_PORT + ";Database=" + DB_NAME + ";User Id=" + DB_USERNAME + ";Password=" + DB_PASSWORD;
            //string connectionString = "Server=localhost;Port=5432;Database=TaskerDB;User Id=postgres;Password=PostgresMoemen";
            return new NpgsqlConnection(@connectionString);
        }




        /// <summary>
        /// Method that Sets up a Client for an OpenFGA store
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        static OpenFgaApi CreateStoreClient(String StoreId)
        {
            var configuration = new Configuration()
            {
                ApiScheme = "http",
                ApiHost = "localhost:8080",
                StoreId = StoreId
            };
            var fgaClient = new OpenFgaApi(configuration);

            return fgaClient;
        }


        /// <summary>
        /// Method that performs a List check
        /// </summary>
        /// <param name="fgaClient"></param>
        /// <param name="AuthorizationModelId"></param>
        /// <param name="tupleUser"></param>
        /// <param name="tupleRelation"></param>
        /// <param name="objectType"></param>
        /// <returns>returns all objects with a specified relationship to a specified user</returns>
        static async Task<ListObjectsResponse> ListCheck(OpenFgaApi fgaClient, String AuthorizationModelId, String tupleUser, String tupleRelation, String objectType)
        {
            var body = new ListObjectsRequest
            {
                AuthorizationModelId = AuthorizationModelId,
                User = tupleUser,
                Relation = tupleRelation,
                Type = objectType,
            };
            var response = await fgaClient.ListObjects(body);

            return response;
        }



    }
}
