using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace SqlLib
{
    public class MsSqlQuery : MsSqlBase, IDbQuery
    {
      
        public MsSqlQuery()
        {
        }
        public MsSqlQuery(string connStr)
        {
            ConnStr = connStr;
        }
        public DataTable ExecuteQuery(IDbCommand cmd)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                cmd.Connection = conn;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd as SqlCommand);
                var dt = new DataTable();
                try
                {
                    adapter.Fill(dt);
                }
                catch (Exception e)
                {
                    ErrorNLog(cmd, e);
                }
                finally
                {
                    adapter.Dispose();
                    cmd.Dispose();
                    conn.Close();
                }
                return dt;
            }
        }

        public DataTable ExecuteQuery(string sqlTxt)
        {
            MsSqlCmdHelper cmdHelper = new MsSqlCmdHelper();
            var cmd = cmdHelper.GetCmd(sqlTxt);
            return ExecuteQuery(cmd);
        }

        public DataSet ExecuteQuery(List<IDbCommand> cmdList)
        {
            using (IDbConnection con = this.GetConnection())
            {
                SqlDataAdapter adp = new SqlDataAdapter();
                DataSet ds = new DataSet();
                try
                {
                    con.Open();

                    foreach (var cmd in cmdList)
                    {
                        try
                        {
                            var dt = new DataTable();
                            cmd.Connection = con;
                            adp.SelectCommand = cmd as SqlCommand;
                            adp.Fill(dt);
                            ds.Tables.Add(dt);
                        }
                        catch (Exception e)
                        {
                            ErrorNLog(cmd, e);
                        }
                    }
                }
                finally
                {
                    con.Close();
                }

                return ds;

            }
        }

        public DataSet ExecuteQuery(List<string> sqlTxtList)
        {
            List<IDbCommand> cmdList = new List<IDbCommand>();
            foreach (string sqlTxt in sqlTxtList)
            {
                IDbCommand cmd = new SqlCommand();
                cmd.CommandText = sqlTxt;
                cmdList.Add(cmd);
            }
            return ExecuteQuery(cmdList);
        }
        private void ErrorNLog(IDbCommand cmd, Exception ex)
        {
            var logger = Logger();
            logger.Trace(cmd.CommandText);
            logger.Trace(ex.ToString);
        }

    }
}
