using System.Configuration;
using System.Data.SqlClient;

namespace refactor_me.Dal.Sql
{
    public class ConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection NewConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}