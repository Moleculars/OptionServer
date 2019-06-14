using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Bb.OptionServer
{

    public class DbDataReaderContext
    {

        public DbDataReader Reader { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool? ReadBool(string columnName)
        {
            return (bool?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeOffset? GetDateTime(string columnName)
        {
            return (DateTimeOffset?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Decimal? GetDecimal(string columnName)
        {
            return (decimal?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double? GetDouble(string columnName)
        {
            return (double?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float? GetFloat(string columnName)
        {
            return (float?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid? GetGuid(string columnName)
        {
            return (Guid?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int16? GetInt16(string columnName)
        {
            return (Int16?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32? GetInt32(string columnName)
        {
            return (Int32?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64? GetInt64(string columnName)
        {
            return (Int64?)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public String GetString(string columnName)
        {
            return (string)GetValue(columnName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (string, object) GetValue(int index)
        {
            return (Reader.GetName(index), Reader.GetValue(index));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetValue(string index)
        {

            var value = Reader.GetValue(Reader.GetOrdinal(index));

            if (value == DBNull.Value)
                return null;

            return value;

        }


    }

}