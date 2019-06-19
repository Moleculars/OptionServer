using System;

namespace Bb.OptionServer.Dao
{

    public class FieldValue
    {

        public object Data { get; set; } = DBNull.Value;

        public object OldData { get; set; } = DBNull.Value;

        public static readonly FieldValue CURRENT_TIMESTAMP = new FieldValue() { Data = "CURRENT_TIMESTAMP", OldData = "CURRENT_TIMESTAMP" };

        public override string ToString()
        {
            return this == FieldValue.CURRENT_TIMESTAMP ? "CURRENT_TIMESTAMP" : Data.ToString();
        }

    }

}
