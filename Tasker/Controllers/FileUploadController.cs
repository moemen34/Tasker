using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OfficeOpenXml;
using System.IO;
using Tasker.Models;
using DotNetEnv;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromForm] FileModel fileModel)
        {
            //Console.WriteLine("gggggggggggg");
            try
            {
                MemoryStream stream = new MemoryStream();
                fileModel.File.CopyTo(stream);

                //Console.WriteLine(fileModel.FileName + "   " + stream.ToArray());

                /*foreach(var item in stream.ToArray())
                {
                    Console.WriteLine(item + " ");
                }*/

                ProcessEmployees(stream);

                return StatusCode(StatusCodes.Status202Accepted);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }


        public static void ProcessEmployees(MemoryStream stream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (MemoryStream ms = stream)
            using (ExcelPackage package = new ExcelPackage(ms))
            {

                /*foreach (var line in package.Workbook.Worksheets)
                {
                    Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH" + line);
                }*/
                //get the first sheet from the excel file
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                //sheet.Cells[0, 0].Value.ToString();


                //loop all rows in the sheet
                for (int i = sheet.Dimension.Start.Row; i <= sheet.Dimension.End.Row; i++)
                {
                    //loop all columns in a row
                    /*for (int j = sheet.Dimension.Start.Column; j <= sheet.Dimension.End.Column; j++)
                    {
                        //do something with the current cell value
                        string currentCellValue = sheet.Cells[i, j].Value.ToString();
                        Console.WriteLine(currentCellValue);
                    }*/

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


        public static bool InsertEmployee(string first_name, string last_name, string email, string phone, string password)
        {

            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");

                    string SQL = "insert into employee(first_name,last_name,email,phone,password) values(:first_name,:last_name,:email,:phone,:password)";
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
                        n = cmd.ExecuteNonQuery();
                        Console.WriteLine("NOT FAIL");
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



        private static NpgsqlConnection GetConnection()
        {
            Env.TraversePath().Load();
            string? DB_HOST = Environment.GetEnvironmentVariable("HOST");
            string? DB_PORT = Environment.GetEnvironmentVariable("PORT");
            string? DB_NAME = Environment.GetEnvironmentVariable("DATABASE");
            string? DB_USERNAME = Environment.GetEnvironmentVariable("USERNAME");
            string? DB_PASSWORD = Environment.GetEnvironmentVariable("PASSWORD");
            string connectionString = "Server=" + DB_HOST + ";Port=" + DB_PORT + ";Database=" + DB_NAME + ";User Id=" + DB_USERNAME + ";Password=" + DB_PASSWORD;
            //string connectionString = "Server=localhost;Port=5432;Database=TaskerDB;User Id=postgres;Password=PostgresMoemen";
            return new NpgsqlConnection(@connectionString);
        }


    }
}
