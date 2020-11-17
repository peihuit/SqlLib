using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SqlLib
{
	public class MsSqlCmdHelper : ISqlCmdHelper
	{
		private List<IDataParameter> _parameters;
		private readonly int _cmdTimeOut = 60;
		public MsSqlCmdHelper()
		{
			
		}
		public string TableName { get; set; }
		public List<IDataParameter> ExtraParameters { get; set; }
		public List<IDataParameter> ExcludeParameters { get; set; }
        public List<string> ExcludeCols { get; set; }

        public void AppendExcludeCol(string colName)
        {
            if (ExcludeCols == null)
                ExcludeCols = new List<string>();
            ExcludeCols.Add(colName);
        }

        public void AppendParameter(IDataParameter dp)
        {
            if (_parameters == null)
                _parameters = new List<IDataParameter>();
            _parameters.Add(dp);
        }
        public IDbCommand GetCmd(string sqlTxt, CommandType cmdType = CommandType.Text)
        {
            var sqlCmd = new SqlCommand();
            sqlCmd.CommandText = sqlTxt;
            sqlCmd.CommandType = cmdType;
            sqlCmd.CommandTimeout = _cmdTimeOut;
            return sqlCmd;
        }
        
		public IDbCommand GetCmd<T>(string sqlTxt, T obj, CommandType cmdType = CommandType.Text)
		{
			if (_parameters == null)
				_parameters = Object2Parameter(obj);
			var sqlCmd = new SqlCommand();
			foreach (var p in _parameters)
			{
				sqlCmd.Parameters.Add(p);
			}

			if (ExtraParameters != null)
			{
				sqlCmd.Parameters.AddRange(ExtraParameters.ToArray());
			}
			sqlCmd.CommandText = sqlTxt;
			sqlCmd.CommandType = cmdType;
			sqlCmd.CommandTimeout = _cmdTimeOut;
			return sqlCmd;
		}
		public string GetInsertSqlTxt<T>(T obj)
		{
			if (TableName == null)
				return "No Table Name";
			if (_parameters == null)
				_parameters = Object2Parameter(obj);
			string fStr = ""; //欄位字串
			string pStr = ""; //參數字串
			foreach (var p in _parameters)
			{
				if (CheckColExclude(p.ParameterName.ToString()))
				{
					continue;
				}
				fStr += p.ParameterName.ToString() + ",";
				pStr += "@" + p.ParameterName.ToString() + ",";
			}
			fStr = fStr.Trim(',');
			pStr = pStr.Trim(',');
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO " + TableName + " (");
			sb.Append(fStr);
			sb.Append(") VALUES (");
			sb.Append(pStr);
			sb.Append(")");
			return sb.ToString();
		}

		public IDbCommand GetInsertCmd<T>(T obj)
		{
			if (_parameters != null)
				_parameters = Object2Parameter(obj);
			var sqlTxt = GetInsertSqlTxt(obj);
			return GetCmd(sqlTxt, obj);
		}

		public bool CheckColExclude(string colName)
		{
            bool checkStatus = false;

			if (ExcludeParameters != null)
			{
				if (ExcludeParameters.Any(v => v.ParameterName.Equals(colName)))
				{
                    checkStatus = true;
				}
			}
            if (ExcludeCols != null)
            {
                if (ExcludeCols.Any(v => v.Contains(colName)))
                {
                    checkStatus = true;
                }
            }
            return checkStatus;
		}
		public string GetUpdateSqlTxt<T>(T obj, string whereStr)
		{
			return GetUpdateSqlTxt(obj, new List<string>() { whereStr });
		}

		public string GetUpdateSqlTxt<T>(T obj, List<string> whereList)
		{
            List<string> cols = new List<string>();
			if (TableName == null)
				return "No Table Name";
			if (_parameters == null)
				_parameters = Object2Parameter(obj);
			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE " + TableName + " SET ");
			foreach (var p in _parameters)
			{
				var colName = p.ParameterName.ToString();
				if (CheckColExclude(colName))
				{
					continue;
				}
				sb.Append(colName + "=@" + colName + ",");
			}

			if (ExtraParameters != null)
			{
				foreach (var p in ExtraParameters)
				{
					var colName = p.ParameterName.ToString();
					if (CheckColExclude(colName))
					{
						continue;
					}
					sb.Append(colName + "=@" + colName + ",");
				}
			}
			sb.Remove(sb.ToString().Length - 1, 1);
			if (whereList != null)
			{
				foreach (var wStr in whereList)
				{
					sb.Append(" WHERE " + wStr);
				}
			}
			return sb.ToString();
		}

		public IDbCommand GetUpdateCmd<T>(T obj, string whereStr)
		{
			return GetUpdateCmd(obj, new List<string>() { whereStr });
		}

		public IDbCommand GetUpdateCmd<T>(T obj, List<string> whereList)
		{
			if (_parameters != null)
				_parameters = Object2Parameter(obj);
			string sqlTxt = GetUpdateSqlTxt(obj, whereList);
			return GetCmd(sqlTxt, obj);
		}

		public string GetDeleteSqlTxt<T>(T obj, string whereStr)
		{
			return GetDeleteSqlTxt(obj, new List<string>() { whereStr });
		}

		public string GetDeleteSqlTxt<T>(T obj, List<string> whereList)
		{
			if (TableName == null)
				return "No Table Name";
			if (_parameters == null)
				_parameters = Object2Parameter(obj);
			StringBuilder sb = new StringBuilder();
			sb.Append("DELETE FROM " + TableName + " WHERE ");
			for (int i = 0; i <= whereList.Count - 1; i++)
			{
				if (i == 0)
				{
					sb.Append(" WHERE " + whereList[i].ToString() + " ");
				}
				else
				{
					sb.Append(" " + whereList[i].ToString() + " ");
				}
			}
			return sb.ToString();
		}

		public IDbCommand GetDeleteCmd<T>(T obj, string whereStr)
		{
			return GetDeleteCmd(obj, new List<string>() { whereStr });
		}

		public IDbCommand GetDeleteCmd<T>(T obj, List<string> whereList)
		{
			if (_parameters == null)
				_parameters = Object2Parameter(obj);
			string sqlTxt = GetDeleteSqlTxt(obj, whereList);
			return GetCmd(sqlTxt, obj);
		}


		public List<IDataParameter> Object2Parameter<T>(T obj)
		{
			List<IDataParameter> parameters = new List<IDataParameter>();
			foreach (PropertyInfo property in obj.GetType().GetProperties())
			{
				if (property.GetValue(obj, null) != null)
				{
					var sqlParameter = new SqlParameter();
					sqlParameter.Value = property.GetValue(obj, null);
					sqlParameter.ParameterName = property.Name;
					sqlParameter.SqlDbType = MsSqlDbTypeMap.GetDbType(property.GetValue(obj, null).GetType());
					parameters.Add(sqlParameter);
				}
			}
			return parameters;
		}
	}
}
