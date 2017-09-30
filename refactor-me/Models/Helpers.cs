using System.Configuration;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    public class Helpers
    {
        public static SqlConnection NewConnection()
        {
            var connstr = ConfigurationManager.ConnectionStrings["EntityStorage"].ConnectionString;
            return new SqlConnection(connstr);
        }
    }
}