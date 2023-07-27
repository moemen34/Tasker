using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OpenFga.Sdk.Model;
using Tasker.Models;
using Tasker.OpenFGA;
using Tasker.Postgres;

namespace Tasker.Controllers
{
    /// <summary>
    /// Method that accepts a get request with an employeeID and returns a list of tasks
    /// the user is allowed to view
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CanViewTaskController : ControllerBase
    {
        [HttpGet]
        public async Task<List<TaskModel>> GetAsync(int employeeId)
        {

            List<TaskModel> Tasks = new List<TaskModel>();

            var fgaClient = FGAMethods.CreateStoreClient();
            ListObjectsResponse response = await FGAMethods.ListCheck(fgaClient, "employee:" + employeeId, "can_view", "task");

            if (response.Objects != null)
            {
                for (int i = 0; i < response.Objects.Count; i++)
                {
                    string? task = response.Objects[i];

                    string ID = task.Split(':')[1];

                    Tasks.Add(GetTask(int.Parse(ID)));

                }
            }
            return Tasks;
        }

        /// <summary>
        /// Method that returns a task model retrieved from the database
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static TaskModel? GetTask(int taskID)
        {
            TaskModel task = new TaskModel();

            NpgsqlCommand command;
            NpgsqlDataReader reader;
            string query;
            
            using (NpgsqlConnection con = PostgreSQL.GetConnection())
            {
                query = $"SELECT first_name, last_name, task_title, complete, assigned_on, due_date FROM task join employee on assigner = employee_id WHERE task_id = {taskID};";
                command = new NpgsqlCommand(@query, con);
                con.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read()) //while to read multiple
                    {
                        //read the data and create a TaskFolder object
                        TaskModel TaskMod = new TaskModel();

                        TaskMod.Assigner = (string)reader["first_name"] + " " + (string)reader["last_name"];
                        TaskMod.TaskTitle = (string)reader["task_title"];
                        TaskMod.Complete = (bool)reader["complete"];
                        TaskMod.AssignedOn = (DateTime)reader["assigned_on"];
                        TaskMod.DueOn = (DateTime)reader["due_date"];

                        reader.Close();
                        con.Close();

                        return TaskMod;
                    }
                }
                return null;
            }
        }

    }
}
