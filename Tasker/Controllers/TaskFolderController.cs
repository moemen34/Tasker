using Microsoft.AspNetCore.Mvc;
using Tasker.Models;

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

        [HttpGet("{employeeId:int}")]
        public List<TaskFolder> Get(int employeeId)
        {
            //TaskFolder[] folders = Folders.ToArray();  
            
            return Folders;
        }


    }
}
