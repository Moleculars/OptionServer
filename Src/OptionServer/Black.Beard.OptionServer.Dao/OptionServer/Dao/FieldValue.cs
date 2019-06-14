using System;
using System.Data;
using System.Xml.Linq;

namespace Bb.OptionServer
{

    public interface IField
    {

        object Value { get; set; }

        object OldValue { get; }

        bool HasModified { get; }

        bool IsDefaultValue(object value);

        void Reset();

        PropertyMapping Properties { get; set; }

    }

    [System.Diagnostics.DebuggerDisplay("{Value} : {OldValue} : {HasModified}")]
    public class FieldValue<T> : IField, IConvertible
    {

        public bool HasModified => !((Value == null && OldValue == null) || Value.Equals(OldValue));

        public T Value { get; set; }

        public T DefaultValue => default(T);

        object IField.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        object IField.OldValue => OldValue;

        public T OldValue { get; private set; }

        public DbType DbType
        {
            get;
        }

        public PropertyMapping Properties { get; set; }

        public object Parent { get; set; }

        public string Name => this.Properties.Name;

        void IField.Reset()
        {
            OldValue = Value;
        }

        public static implicit operator T(FieldValue<T> t) { return t.Value; }

        public static implicit operator FieldValue<T>(T t)
        {
            var result = new FieldValue<T> { Value = t };
            return result;
        }

        public static bool operator ==(T left, FieldValue<T> right)
        {
            return left.Equals(right.Value);
        }

        public static bool operator !=(T left, FieldValue<T> right)
        {
            return !left.Equals(right.Value);
        }

        public override bool Equals(object obj)
        {

            if (Value == null)
            {

                if (obj == null)
                    return true;

                return false;

            }

            if (obj is T v1)
                return Value.Equals(v1);

            return false;

        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        private DbType DbTypeImpl()
        {

            if (_dbType == null)
            {

                Type type = typeof(T);

                if (type == typeof(string))
                    _dbType = DbType.String;

                else if (type == typeof(bool))
                    _dbType = DbType.Boolean;

                else if (type == typeof(DateTime))
                    _dbType = DbType.DateTime;

                else if (type == typeof(DateTimeOffset))
                    _dbType = DbType.DateTimeOffset;

                else if (type == typeof(decimal))
                    _dbType = DbType.Decimal;

                else if (type == typeof(double))
                    _dbType = DbType.Double;

                else if (type == typeof(Guid))
                    _dbType = DbType.Guid;

                else if (type == typeof(Int16))
                    _dbType = DbType.Int16;

                else if (type == typeof(Int32))
                    _dbType = DbType.Int32;

                else if (type == typeof(Int64))
                    _dbType = DbType.Int64;

                else if (type == typeof(Single))
                    _dbType = DbType.Single;

                else if (type == typeof(TimeSpan))
                    _dbType = DbType.Time;

                else if (type == typeof(UInt16))
                    _dbType = DbType.UInt16;

                else if (type == typeof(UInt32))
                    _dbType = DbType.UInt32;

                else if (type == typeof(UInt64))
                    _dbType = DbType.UInt64;

                else if (type == typeof(XElement))
                    _dbType = DbType.Xml;

            }

            return _dbType.Value;

        }

        public TypeCode GetTypeCode()
        {

            if (typeof(T) == typeof(bool))
                return TypeCode.Boolean;

            if (typeof(T) == typeof(Byte))
                return TypeCode.Byte;

            if (typeof(T) == typeof(Char))
                return TypeCode.Char;

            if (typeof(T) == typeof(DateTime))
                return TypeCode.DateTime;

            if (typeof(T) == typeof(Decimal))
                return TypeCode.Decimal;

            if (typeof(T) == typeof(Double))
                return TypeCode.Double;

            if (typeof(T) == typeof(Int16))
                return TypeCode.Int16;

            if (typeof(T) == typeof(Int32))
                return TypeCode.Int32;

            if (typeof(T) == typeof(Int64))
                return TypeCode.Int64;

            if (typeof(T) == typeof(SByte))
                return TypeCode.SByte;

            if (typeof(T) == typeof(Single))
                return TypeCode.Single;

            if (typeof(T) == typeof(string))
                return TypeCode.String;

            if (typeof(T) == typeof(UInt16))
                return TypeCode.UInt16;

            if (typeof(T) == typeof(UInt32))
                return TypeCode.UInt32;

            if (typeof(T) == typeof(UInt64))
                return TypeCode.UInt64;

            if (typeof(T) == typeof(ReadOnlyException))
                return TypeCode.Object;

            return TypeCode.Empty;
        }

        #region IConvertible

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(Value, provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(Value, provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(Value, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(Value, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(Value, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Value, provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value, provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(Value, provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(Value, provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(Value, provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(Value, provider);

        }

        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(Value, provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Value, conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(Value, provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(Value, provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(Value, provider);
        }

        #endregion IConvertible

        public bool IsDefaultValue(object value)
        {
            return default(T).Equals(value);
        }


        private DbType? _dbType;

    }

}