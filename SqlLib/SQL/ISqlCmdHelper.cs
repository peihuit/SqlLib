using System.Collections.Generic;
using System.Data;

namespace SqlLib
{
	public interface ISqlCmdHelper
	{
		string TableName { get; set; }
		List<IDataParameter> ExtraParameters { get; set; }
		List<IDataParameter> ExcludeParameters { get; set; }
        List<string> ExcludeCols { get; set; }
        void AppendExcludeCol(string colName);

        IDbCommand GetCmd(string sqlTxt, CommandType cmdType = CommandType.Text);
		IDbCommand GetCmd<T>(string sqlTxt, T obj, CommandType cmdType = CommandType.Text);

		string GetInsertSqlTxt<T>(T obj);
		IDbCommand GetInsertCmd<T>(T obj);

		string GetUpdateSqlTxt<T>(T obj, string whereStr);
		string GetUpdateSqlTxt<T>(T obj, List<string> whereList);
		IDbCommand GetUpdateCmd<T>(T obj, string whereStr);
		IDbCommand GetUpdateCmd<T>(T obj, List<string> whereList);

		string GetDeleteSqlTxt<T>(T obj, string whereStr);
		string GetDeleteSqlTxt<T>(T obj, List<string> whereList);
		IDbCommand GetDeleteCmd<T>(T obj, string whereStr);
		IDbCommand GetDeleteCmd<T>(T obj, List<string> whereList);

		List<IDataParameter> Object2Parameter<T>(T obj);

	}
}
