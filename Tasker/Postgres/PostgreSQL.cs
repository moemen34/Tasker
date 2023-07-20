using DotNetEnv;
using Npgsql;

namespace Tasker.Postgres
{
    public class PostgreSQL
    {
        public static NpgsqlConnection GetConnection()
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
