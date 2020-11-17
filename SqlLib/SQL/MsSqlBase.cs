using NLog;
using System.Configuration;
using System.Data.SqlClient;

namespace SqlLib
{
    public class MsSqlBase
    {
      
        private string _connStr = ConfigurationManager.ConnectionStrings["DefaultConn"].ToString();
        public string ConnStr { get; set; }
        public SqlConnection GetConnection()
        {
            if (!string.IsNullOrEmpty(ConnStr))
                _connStr = ConnStr;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _connStr;
            return conn;
        }

        public Logger Logger()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            return logger;
        }
    }
}
