using SqlLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace SqlLib
{
    public class MsSqlNonQuery : MsSqlBase, IDbNonQuery
    {
        
        public MsSqlNonQuery()
        {
        }

        public MsSqlNonQuery(string connStr)
        {
            ConnStr = connStr;
        }
        public RVal ExecuteNonQuery(string sqlTxt)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = sqlTxt;
            return ExecuteNonQuery(cmd);
        }

        public RVal ExecuteNonQuery(List<string> sqlTxtList)
        {
            List<IDbCommand> cmdList = new List<IDbCommand>();
            foreach (string sqlTxt in sqlTxtList)
            {
                var cmd = new SqlCommand();
                cmd.CommandText = sqlTxt;
                cmdList.Add(cmd);
            }

            return ExecuteNonQuery(cmdList);
        }

        public RVal ExecuteNonQuery(IDbCommand cmd)
        {
            using (SqlConnection conn = this.GetConnection())
            {
                RVal rval = new RVal();
                CommittableTransaction ct = new CommittableTransaction();
                conn.Open();
                conn.EnlistTransaction(ct);
                cmd.Connection = conn;
                try
                {
                    rval.RStatus = true;
                    cmd.ExecuteNonQuery();
                    ct.Commit();
                }
                catch (Exception ex)
                {
                    rval.RStatus = false;
                    ErrorNLog(cmd, ex);
                    ct.Rollback();
                }
                finally
                {
                    conn.Close();
                }

                return rval;
            }
        }

        public RVal ExecuteNonQuery(List<IDbCommand> cmdList)
        {
            using (SqlConnection conn = this.GetConnection())
            {
                RVal rval = new RVal();
                CommittableTransaction ct = new CommittableTransaction();
                conn.Open();
                conn.EnlistTransaction(ct);
                try
                {
                    foreach (var cmd in cmdList)
                    {
                        try
                        {
                            rval.RStatus = true;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            rval.RStatus = false;
                            ErrorNLog(cmd, ex);
                            ct.Rollback();
                            return rval;
                        }
                    }
                    ct.Commit();
                }
                finally
                {
                    conn.Close();
                }
                return rval;
            }
        }

        public RVal ExecuteScalar(IDbCommand cmd)
        {
            using (SqlConnection conn = this.GetConnection())
            {
                RVal rval = new RVal();
                CommittableTransaction ct = new CommittableTransaction();
                conn.Open();
                conn.EnlistTransaction(ct);
                cmd.Connection = conn;
                try
                {
                    var srObj = cmd.ExecuteScalar();
                    rval.RStatus = true;
                    rval.DVal = srObj;
                    ct.Commit();
                }
                catch (Exception ex)
                {
                    rval.RStatus = false;
                    ErrorNLog(cmd, ex);
                    ct.Rollback();
                }
                finally
                {
                    conn.Close();
                }

                return rval;
            }
        }

        private void ErrorNLog(IDbCommand cmd, Exception ex)
        {
            var logger = Logger();
            logger.Trace(cmd.CommandText);
            logger.Trace(ex.ToString);
        }
    }
}
