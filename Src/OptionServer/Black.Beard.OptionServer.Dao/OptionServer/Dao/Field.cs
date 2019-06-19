using System.Data;

namespace Bb.OptionServer.Dao
{


    [System.Diagnostics.DebuggerDisplay("{Name} : {Value}")]
    public class Field
    {

        public string Name { get; set; }

        public FieldValue Value { get; set; }

        public DbType DbType { get; set; }

        public string VariableName { get; set; }

    }

}
