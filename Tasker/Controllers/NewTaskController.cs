using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Tasker.Models;
using Tasker.OpenFGA;
using Tasker.Postgres;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewTaskController : ControllerBase
    {
        [HttpPost]
        public /*ActionResult*/ string[] Post(NewTask task)
        {
            //Console.WriteLine("gggggggggggg");
            try
            {
                //store in database:
                //task and


                //task_info


                //add edges to openFGA
                //task:id       assigner          employee:id
                //task:id       parentFolder      taskFolder:assignee


                foreach (var Assignee in task.Assignees) 
                { 
                    Console.WriteLine(Assignee);
                }

                Console.WriteLine(task.DueDate);
                Console.WriteLine(task.AssignerID);

                return task.Assignees;//StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception)
            {
                return task.Assignees;//StatusCode(StatusCodes.Status500InternalServerError);
            }

        }



        public static async Task<bool> InsertTask(string first_name, string last_name, string email, string phone, string password)
        {

            using (NpgsqlConnection con = PostgreSQL.GetConnection())
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");

                    string SQL = "insert into employee(first_name,last_name,email,phone,password) values(:first_name,:last_name,:email,:phone,:password) RETURNING employee_id";
                    NpgsqlCommand cmd = new NpgsqlCommand(SQL);
                    cmd.Connection = con;

                    cmd.Parameters.Add(new NpgsqlParameter(":first_name", NpgsqlTypes.NpgsqlDbType.Text));
                    cmd.Parameters[0].Value = first_name;

                    cmd.Parameters.Add(new NpgsqlParameter(":last_name", NpgsqlTypes.NpgsqlDbType.Text));
                    cmd.Parameters[1].Value = last_name;

                    cmd.Parameters.Add(new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Text));
                    cmd.Parameters[2].Value = email;

                    cmd.Parameters.Add(new NpgsqlParameter(":phone", NpgsqlTypes.NpgsqlDbType.Text));
                    cmd.Parameters[3].Value = phone;

                    cmd.Parameters.Add(new NpgsqlParameter(":password", NpgsqlTypes.NpgsqlDbType.Text));
                    cmd.Parameters[4].Value = password;

                    int n;


                    try
                    {
                        n = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine("NOT FAIL");

                        await FGAMethods.AddRelationAsync("employee:" + n, "task_folder:" + n, "owner");


                    }
                    catch (PostgresException pgE)
                    {
                        Console.WriteLine("FAIL " + pgE.Message);
                    }
                    con.Close();

                    return true;
                }
                else
                {
                    Console.WriteLine("NOPE");
                }
            }


            return false;
        }







    }
}
