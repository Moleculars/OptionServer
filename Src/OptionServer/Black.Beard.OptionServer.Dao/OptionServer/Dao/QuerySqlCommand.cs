using System.Collections.Generic;

namespace Bb.OptionServer.Dao
{

    public class QuerySqlCommand
    {

        public QuerySqlCommand()
        {
            Fields = new List<Field>();
            FilterFields = new List<Field>();
        }

        public QueryKindEnum Kind { get; set; }

        public string Table { get; set; }

        public List<Field> Fields { get; set; }

        public List<Field> FilterFields { get; set; }

    }

}
