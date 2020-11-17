using System.Collections.Generic;
using System.Data;

namespace SqlLib
{
    public interface IDbQuery
    {
        DataTable ExecuteQuery(IDbCommand cmd);
        DataTable ExecuteQuery(string sqlTxt);
        DataSet ExecuteQuery(List<IDbCommand> cmdList);
        DataSet ExecuteQuery(List<string> sqlTxtList);
    }
}
