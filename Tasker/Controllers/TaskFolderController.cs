using Microsoft.AspNetCore.Mvc;
using Tasker.Models;
using Tasker.OpenFGA;
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

        /// <summary>
        /// Get Request that receives an employee id and requested relation type and performs an OpenFGA list check
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="relation">if 1 => returns supervisor_plus list check, 
        ///                           2 => returns assistant list check,
        ///                           3 => returns can_assign list check</param>
        /// <returns>r</returns>
        [HttpGet]
        public async Task<List<TaskFolder>> GetAsync(int employeeId, int relation)
        {
           
            List<TaskFolder> folders = new List<TaskFolder>();

            var fgaClient = FGAMethods.CreateStoreClient();

            ListObjectsResponse response = new ListObjectsResponse();

            //use enum instead of 1 and 2
            if (relation == 1)
                response = await FGAMethods.ListCheck(fgaClient, "employee:" + employeeId, "supervisor_plus", "employee");
            else if(relation == 2)
                response = await FGAMethods.ListCheck(fgaClient, "employee:" + employeeId, "assistant", "employee");
            else if(relation == 3)
                response = await FGAMethods.ListCheck(fgaClient, "employee:" + employeeId, "can_assign", "task_folder");

            if (response.Objects != null)
            {
                for (int i = 0; i < response.Objects.Count; i++)
                {
                    string? employee = response.Objects[i];

                    string ID = employee.Split(':')[1];

                    folders.Add(GetTaskFolder(ID));

                }

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
            return new NpgsqlConnection(@connectionString);
        }

    }
}
