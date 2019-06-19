using System;
using System.Data;

namespace Bb.OptionServer
{



    public class SqlServerPredicateGenerator : IQueryPredicateGenerator
    {

        private SqlServerPredicateGenerator()
        {

        }

        static SqlServerPredicateGenerator()
        {
            Instance = new SqlServerPredicateGenerator();
        }


        public static IQueryPredicateGenerator Instance { get; }


        public string CurrentTimestamp => "CURRENT_TIMESTAMP";

        public string WriteEquality()
        {
            return " = ";
        }

        public string WriteMember(string name)
        {
            var i = name.Split('.');
            return "[" + string.Join("].[", i) + "]";
        }

        public string WriteNotEquality()
        {
            return " <> ";
        }

        public string WriteParameter(string variableName)
        {
            return "@" + variableName;
        }

        public string WriteType(DbType variableType, object value)
        {

            switch (variableType)
            {
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    return $"VARCHAR({value.ToString().Length})";

                case DbType.Binary:
                    var length = (value as Array).Length;
                    return $"BINARY({length})";

                case DbType.Boolean:
                    return "BIT";

                case DbType.Currency:
                    return "MONEY";

                case DbType.Date:
                    return "DATE";

                case DbType.DateTime:
                    return "DATETIME";

                case DbType.DateTime2:
                    return "DATETIME2";

                case DbType.DateTimeOffset:
                    return "DATETIMEOFFSET";

                case DbType.Time:
                    return "TIME";

                case DbType.Single:
                case DbType.Decimal:
                    return "DECIMAL";

                case DbType.Double:
                    return "FLOAT";

                case DbType.Guid:
                    return "UNIQUEIDENTIFIER";

                case DbType.Byte:
                case DbType.SByte:
                    return "TINYINT";

                case DbType.Int16:
                case DbType.UInt16:
                    return "SMALLINT";

                case DbType.UInt32:
                case DbType.Int32:
                    return "INT";

                case DbType.UInt64:
                case DbType.Int64:
                    return "BIGINT";

                case DbType.VarNumeric:
                    return "NUMERIC";

                case DbType.Object:
                    break;

                case DbType.Xml:
                    break;
                default:
                    break;
            }

            throw new NotImplementedException(variableType.ToString());

        }

        public string WriteStringValue(object value)
        {
            return $"'{value}'";
        }

        public string WriteBooleanValue(object value)
        {
            var t1 = (bool)value ? 1 : 0;
            return t1.ToString();
        }

        public string WriteGuid(object value)
        {
            var t2 = (Guid)value;
            return $"'{t2.ToString("D")}'";
        }

        public string WriteDatetime(object value)
        {
            var t3 = (DateTime)value;
            return $"'{t3.ToString("u")}'";
        }

        public string WriteTimeOffset(object value)
        {
            var t4 = (DateTime)value;
            return $"'{t4.ToString("u")}'";
        }

        public string WriteTime(object value)
        {
            var t5 = (TimeSpan)value;
            return $"'{t5.ToString("G")}'";
        }

        public string WriteDecimal(object value)
        {
            return value.ToString();
        }

        public string WriteInt(object value)
        {
            return value.ToString();
        }

        public string WriteUInt(object value)
        {
            return value.ToString();
        }

    }

    public interface IQueryPredicateGenerator
    {
        string CurrentTimestamp { get; }

        string WriteMember(string name);

        string WriteParameter(string variableName);

        string WriteEquality();

        string WriteNotEquality();
        string WriteType(DbType variableType, object value);

        string WriteStringValue(object value);
        string WriteBooleanValue(object value);
        string WriteGuid(object value);
        string WriteDatetime(object value);
        string WriteTimeOffset(object value);
        string WriteTime(object value);
        string WriteDecimal(object value);
        string WriteInt(object value);
        string WriteUInt(object value);

    }

}