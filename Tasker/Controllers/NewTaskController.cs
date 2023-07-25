using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Npgsql;
using System.Security.Cryptography.Pkcs;
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



                InsertTask(task.AssignerID, task.TaskTitle, task.DueDate, task.Assignees);



                return task.Assignees;//StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception)
            {
                return task.Assignees;//StatusCode(StatusCodes.Status500InternalServerError);
            }

        }



        public static async Task<bool> InsertTask(int assigner, string taskTitle, DateTime dueDate, string[] assignees)
        {

            using (NpgsqlConnection con = PostgreSQL.GetConnection())
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");

                    string SQL = "insert into task(assigner, task_title, complete, due_date) values(:assigner,:task_title, :complete, :due_date) RETURNING task_id";
                    NpgsqlCommand cmd = new NpgsqlCommand(SQL);
                    cmd.Connection = con;

                    cmd.Parameters.Add(new NpgsqlParameter(":assigner", NpgsqlTypes.NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = assigner;

                    cmd.Parameters.Add(new NpgsqlParameter(":task_title", NpgsqlTypes.NpgsqlDbType.Text));
                    cmd.Parameters[1].Value = taskTitle;

                    cmd.Parameters.Add(new NpgsqlParameter(":complete", NpgsqlTypes.NpgsqlDbType.Boolean));
                    cmd.Parameters[2].Value = false;

                    cmd.Parameters.Add(new NpgsqlParameter(":due_date", NpgsqlTypes.NpgsqlDbType.Timestamp));
                    cmd.Parameters[3].Value = dueDate;



                    int n;


                    try
                    {
                        n = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine("NOT FAIL");

                        await FGAMethods.AddRelationAsync("employee:" + assigner, "task:" + n, "assigner");

                        await InsertTaskInfo(n, assignees);

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


        public static async Task<bool> InsertTaskInfo(int taskID, string[] assignees)
        {

            using (NpgsqlConnection con = PostgreSQL.GetConnection())
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");

                    foreach (string assignee in assignees)
                    {
                        string ThisAssignee = assignee.Substring(1, assignee.Length-2);
                        string SQL = "insert into task_info(task_id, assignee, status_per_assignee, viewed) values(:task_id, :assignee, :status_per_assignee, :viewed)";
                        NpgsqlCommand cmd = new NpgsqlCommand(SQL);
                        cmd.Connection = con;

                        cmd.Parameters.Add(new NpgsqlParameter(":task_id", NpgsqlTypes.NpgsqlDbType.Integer));
                        cmd.Parameters[0].Value = taskID;

                        cmd.Parameters.Add(new NpgsqlParameter(":assignee", NpgsqlTypes.NpgsqlDbType.Integer));
                        cmd.Parameters[1].Value = int.Parse(ThisAssignee);

                        cmd.Parameters.Add(new NpgsqlParameter(":status_per_assignee", NpgsqlTypes.NpgsqlDbType.Boolean));
                        cmd.Parameters[2].Value = false;

                        cmd.Parameters.Add(new NpgsqlParameter(":viewed", NpgsqlTypes.NpgsqlDbType.Boolean));
                        cmd.Parameters[3].Value = false;



                        int n;


                        try
                        {
                            n = cmd.ExecuteNonQuery();
                            Console.WriteLine("NOT FAIL");

                            //foreach (string assignee in assignees)
                            //{
                            await FGAMethods.AddRelationAsync("task_folder:" + ThisAssignee, "task:" + taskID, "parent_folder");
                            //}

                        }
                        catch (PostgresException pgE)
                        {
                            Console.WriteLine("FAIL " + pgE.Message);
                        }
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
