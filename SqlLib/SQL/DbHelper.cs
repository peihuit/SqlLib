using SqlLib.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlLib
{
    /// <summary>
    /// IDbManager 的摘要描述
    /// </summary>
    public class DbHelper : IDbNonQuery, IDbQuery
    {
        private string _connStr = "";
        private IDbQuery _dbQuery;
        private IDbNonQuery _dbNonQuery;
        private DapperHelper _dapperHelper;
        private ISqlCmdHelper _cmdHelper;
        private IDbCommand _dbCmd;
        public DbHelper()
        {
        }

        public DapperHelper DapperHelper
        {
            get
            {
                if (_dapperHelper == null)
                    _dapperHelper = new DapperHelper();
                return _dapperHelper;
            }
        }
        public DbHelper(string connStr)
        {
            _connStr = connStr;
        }

        public IDbCommand GetDbCmd()
        {
            if (_dbCmd == null)
                _dbCmd = new SqlCommand();
            return _dbCmd;
        }
        public IDbQuery GetDbQuery()
        {
            if (_dbQuery == null)
            {
                if (_connStr == "")
                    _dbQuery = new MsSqlQuery();
                else
                    _dbQuery = new MsSqlQuery(_connStr);
            }
            return _dbQuery;
        }
        public IDbNonQuery GetNonQuery()
        {
            if (_dbNonQuery == null)
            {
                if (_connStr == "")
                    _dbNonQuery = new MsSqlNonQuery();
                else
                    _dbNonQuery = new MsSqlNonQuery(_connStr);
            }
            return _dbNonQuery;
        }
        public ISqlCmdHelper GetCmdHelper()
        {
            if (_cmdHelper == null)
                _cmdHelper = new MsSqlCmdHelper();
            return _cmdHelper;
        }
        public DataTable ExecuteQuery(IDbCommand cmd)
        {
            GetDbQuery();
            return _dbQuery.ExecuteQuery(cmd);
        }
        public DataTable ExecuteQuery(string sqlTxt)
        {
            GetDbQuery();
            return _dbQuery.ExecuteQuery(sqlTxt);
        }
        public DataSet ExecuteQuery(List<IDbCommand> cmdList)
        {
            GetDbQuery();
            return _dbQuery.ExecuteQuery(cmdList);
        }
        public DataSet ExecuteQuery(List<string> sqlTxtList)
        {
            GetDbQuery();
            return _dbQuery.ExecuteQuery(sqlTxtList);
        }

        public RVal ExecuteNonQuery(string sqlTxt)
        {
            GetNonQuery();
            return _dbNonQuery.ExecuteNonQuery(sqlTxt);
        }
        public RVal ExecuteNonQuery(List<string> sqlTxtList)
        {
            GetNonQuery();
            return _dbNonQuery.ExecuteNonQuery(sqlTxtList);
        }
        public RVal ExecuteNonQuery(IDbCommand cmd)
        {
            GetNonQuery();
            return _dbNonQuery.ExecuteNonQuery(cmd);
        }
        public RVal ExecuteNonQuery(List<IDbCommand> cmdList)
        {
            GetNonQuery();
            return _dbNonQuery.ExecuteNonQuery(cmdList);
        }
        public RVal ExecuteScalar(IDbCommand cmd)
        {
            GetNonQuery();
            return _dbNonQuery.ExecuteScalar(cmd);
        }
    }
}