using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OfficeOpenXml;
using Tasker.Models;
using Tasker.OpenFGA;
using Tasker.Postgres;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesFileUploadController : ControllerBase
    {
        /// <summary>
        /// Post request that accepts an excel file with employees information
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

                ProcessEmployees(stream);

                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        /// <summary>
        /// Method that accepts an excel file as a memory stream and processes it's cells
        /// in order to collect data
        /// </summary>
        /// <param name="stream"></param>
        public static void ProcessEmployees(MemoryStream stream)
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

                    Console.WriteLine("reading record");

                    InsertEmployee(sheet.Cells[i, sheet.Dimension.Start.Column].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 1].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 2].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 3].Value.ToString(),
                        sheet.Cells[i, sheet.Dimension.Start.Column + 4].Value.ToString());


                    Console.WriteLine("record sent to insert");

                }
            }
        }

        /// <summary>
        /// Method that inserts an employee into the database
        /// </summary>
        /// <param name="first_name"></param>
        /// <param name="last_name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<bool> InsertEmployee(string first_name, string last_name, string email, string phone, string password)
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

                        await FGAMethods.AddRelationAsync("employee:"+n, "task_folder:" + n , "owner");


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
