using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Tasker.Models;
using Tasker.OpenFGA;


namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationsFileUploadController : ControllerBase
    {
        /// <summary>
        /// Post request that accepts a excel file with employee relations and processes it
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromForm] FileModel fileModel)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                fileModel.File.CopyTo(stream);

                ProcessRelations(stream);

                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Method that processes the contents of the excel file and adds corresponding relations to OpenFGA
        /// </summary>
        /// <param name="stream"></param>
        public static async void ProcessRelations(MemoryStream stream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (MemoryStream ms = stream)
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                //get the first sheet from the excel file
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                //loop all rows in the sheet
                for (int i = sheet.Dimension.Start.Row; i <= sheet.Dimension.End.Row; i++)
                {
                    //loop all columns in a row
                    for (int j = sheet.Dimension.Start.Column; j <= sheet.Dimension.End.Column; j++)
                    {
                        //do something with the current cell value
                        string currentCellValue = sheet.Cells[i, j].Value.ToString();
                        Console.WriteLine(currentCellValue);
                    }

                    Console.WriteLine("reading record");

                    await FGAMethods.AddRelationAsync("employee:" + sheet.Cells[i, sheet.Dimension.Start.Column].Value.ToString(),
                        "employee:" + sheet.Cells[i, sheet.Dimension.Start.Column + 1].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 2].Value.ToString().ToLower());

                    Console.WriteLine("record sent to insert");

                }
            }
        }

    }
}
