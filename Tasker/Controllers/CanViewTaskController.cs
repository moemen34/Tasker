using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
using Tasker.Models;
using Tasker.OpenFGA;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CanViewTaskController : ControllerBase
    {


        //[HttpGet("{employeeId:int}")]
        [HttpGet]
        public async Task<List<TaskFolder>> GetAsync(int employeeId)
        //public async Task<ListObjectsResponse> GetAsync(int employeeId)
        {

            List<TaskFolder> folders = new List<TaskFolder>();

            var fgaClient = FGAMethods.CreateStoreClient();

            ListObjectsResponse response = new ListObjectsResponse();

            //use enum instead of 1 and 2
            //if (relation == 1)
            response = await FGAMethods.ListCheck(fgaClient, "employee:" + employeeId, "supervisor_plus", "employee");
            //else if (relation == 2)
            //response = await ListCheck(fgaClient, "01H5B0VND3034JA8BJP4GBMWH7", "employee:" + employeeId, "assistant", "employee");

            if (response.Objects != null)
            {
                for (int i = 0; i < response.Objects.Count; i++)
                {
                    string? employee = response.Objects[i];

                    string ID = employee.Split(':')[1];

                    //folders.Add(GetTaskFolder(ID));

                }

                /*foreach (var folder in folders)
                {
                    Console.WriteLine(folder.OwnerId + " " + folder.OwnerName + " " + folder.OwnerEmail);
                }*/

            }
            return folders;

        }

    }

}
