using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Tasker.Models;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
using ConsoleApp1;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationsFileUploadController : ControllerBase
    {
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

                    /*InsertEmployee(sheet.Cells[i, sheet.Dimension.Start.Column].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 1].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 2].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 3].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 4].Value.ToString());*/

                    await AddRelationAsync(sheet.Cells[i, sheet.Dimension.Start.Column].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 1].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 2].Value.ToString().ToLower());

                    Console.WriteLine("record sent to insert");

                }
            }
        }


        public static async Task<FgaTaskResult> AddRelationAsync(string sourceID, string destinationID, string relation)
        {
            var fgaClient = CreateStoreClient("01H1AM5QQYN9VZTJ8MNW2HXAJV");

            FgaTaskResult addResult = await AddNewRelationshipTuple(fgaClient, "01H5B0VND3034JA8BJP4GBMWH7", "employee:" + sourceID, relation, "employee:" + destinationID);


            return addResult;
        }

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

        static async Task<FgaTaskResult> AddNewRelationshipTuple(OpenFgaApi fgaClient, string AuthorizationModelId, string tupleUser, string tupleRelation, string tupleObject)
        {
            try
            {
                Console.WriteLine(tupleUser + " "+ tupleRelation + " "+ tupleObject);
                var response = await fgaClient.Write(new WriteRequest
                {
                    Writes = new TupleKeys(new List<TupleKey>() {
                    new() { User = tupleUser, Relation = tupleRelation, Object = tupleObject}
                }),
                    AuthorizationModelId = AuthorizationModelId
                });
                return new FgaTaskResult() { Status = true };//"Success";

            }
            catch (Exception ex) { 
                return new FgaTaskResult() { Status = false, Message = ex.Message }; 
            }
        }

    }
}
