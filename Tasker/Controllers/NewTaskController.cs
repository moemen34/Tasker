using Microsoft.AspNetCore.Mvc;
using Tasker.Models;

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
    }
}
