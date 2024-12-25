using Microsoft.Data.SqlClient;
namespace HQSoft_EX01.Configurations
{
    public class ConnectionHelper
    {
        public static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        // Build the connection string from the environment. i.e. Railway
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            Console.WriteLine($"Đang cố gắng kết nối bằng: {databaseUri}");
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = $"{databaseUri.Host},{databaseUri.Port}", // SQL Server sử dụng định dạng host,port
                InitialCatalog = databaseUri.LocalPath.TrimStart('/'),
                UserID = userInfo[0],
                Password = userInfo[1],
                Encrypt = true,
                TrustServerCertificate = true
            };
            return builder.ConnectionString;
        }

    }
}
