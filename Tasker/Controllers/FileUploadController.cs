using Microsoft.AspNetCore.Mvc;
using System.IO;
using Tasker.Models;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromForm] FileModel fileModel)
        {
            Console.WriteLine("gggggggggggg");
            try
            {
                MemoryStream stream = new MemoryStream();
                fileModel.File.CopyTo(stream);

                Console.WriteLine(fileModel.FileName + "   " + stream.ToArray());

                foreach(var item in stream.ToArray())
                {
                    Console.WriteLine(item + " ");
                }

                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
