using BackEnd.Interface.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace BackEnd.Data
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = _configuration["ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Cadena de conexión no configurada");
            }

            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}