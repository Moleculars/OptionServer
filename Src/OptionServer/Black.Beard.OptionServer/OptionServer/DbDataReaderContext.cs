using System;
using System.Data.Common;

namespace Bb.OptionServer
{

    public class DbDataReaderContext
    {


        public DbDataReader Reader { get; set; }

        public bool ReadBool(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetBoolean(i);
        }

        public DateTimeOffset GetDateTime(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return (DateTimeOffset)Reader.GetValue(i);
        }

        public Decimal GetDecimal(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetDecimal(i);
        }

        public Double GetDouble(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetDouble(i);
        }

        public float GetFloat(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetFloat(i);
        }

        public Guid GetGuid(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetGuid(i);
        }

        public Int16 GetInt16(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetInt16(i);
        }

        public Int32 GetInt32(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetInt32(i);
        }

        public Int64 GetInt64(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetInt64(i);
        }

        public String GetString(string columnName)
        {
            var i = Reader.GetOrdinal(columnName);
            return Reader.GetString(i);
        }



    }

}