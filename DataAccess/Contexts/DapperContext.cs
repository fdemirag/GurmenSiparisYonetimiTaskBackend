using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace DataAccess
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PostgresConnection"); // PostgreSQL bağlantı stringi
        }

        // PostgreSQL bağlantısı oluşturma
        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}