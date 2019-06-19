using System.Data.Common;
using System.Text;

namespace Bb.OptionServer.Dao
{

    public class QueryCommand
    {

        public QueryCommand()
        {

        }

        public QueryCommand(StringBuilder commandText, System.Data.Common.DbParameter[] dbParameter)
        {
            CommandText = commandText;
            Arguments = dbParameter;
        }

        public StringBuilder CommandText { get; set; }

        public DbParameter[] Arguments { get; set; }

        public QueryCommand Rollback { get; set; }

    }

}
