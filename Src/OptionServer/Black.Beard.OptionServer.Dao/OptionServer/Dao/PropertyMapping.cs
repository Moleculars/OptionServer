using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Bb.OptionServer
{
    [System.Diagnostics.DebuggerDisplay("{Name} {Type} Primary : {IsPrimaryKey}")]
    public class PropertyMapping
    {

        internal PropertyMapping(PropertyInfo item)
        {
            _property = item;
            FieldName = Name = item.Name;
            Type = item.PropertyType;
            _validators = new List<ValidationAttribute>();
        }

        public string Name { get; }

        public Type Type { get; }

        public bool IsPrimaryKey { get; private set; }

        public string FieldName { get; private set; }

        public string VariableName { get; private set; }

        public DbType DbType { get; private set; }
        public bool LastChangeDate { get; private set; }
        public bool SecurityCoherence { get; private set; }


        public bool IsGuid()
        {
            return Type == typeof(Guid) || Type == typeof(FieldValue<Guid>);
        }

        public void Validate(object instance)
        {

            var data = _property.GetValue(instance);
            if (data is IField f)
                data = f.Value;

            foreach (var item in _validators)
                item.Validate(data, FieldName);

        }

        internal void Build()
        {

            //var acc = this._property.GetAccessors();
            ResolveDbType();

            var attributes = _property.GetCustomAttributes(true);
            foreach (Attribute attribute in attributes)
            {

                if (attribute is KeyAttribute)
                    IsPrimaryKey = true;

                else if (attribute is ValidationAttribute validator)
                    _validators.Add(validator);

                else if (attribute is ColumnAttribute c)
                    FieldName = c.Name;

                else if (attribute is LastChangeDateAttribute)
                    LastChangeDate = true;

                else if (attribute is SecurityCoherenceAttribute)
                    SecurityCoherence = true;

            }

            VariableName = FieldName[0].ToString().ToLower() + string.Join(string.Empty, FieldName.Skip(1).ToArray());

        }

        private void ResolveDbType()
        {

            Type _type = Type;
            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(FieldValue<>))
                _type = _type.GetGenericArguments()[0];


            if (_type == typeof(string))
                DbType = DbType.String;

            else if (_type == typeof(bool))
                DbType = DbType.Boolean;

            else if (_type == typeof(DateTime))
                DbType = DbType.DateTime;

            else if (_type == typeof(DateTimeOffset))
                DbType = DbType.DateTimeOffset;

            else if (_type == typeof(decimal))
                DbType = DbType.Decimal;

            else if (_type == typeof(double))
                DbType = DbType.Double;

            else if (_type == typeof(Guid))
                DbType = DbType.Guid;

            else if (_type == typeof(Int16))
                DbType = DbType.Int16;

            else if (_type == typeof(Int32))
                DbType = DbType.Int32;

            else if (_type == typeof(Int64))
                DbType = DbType.Int64;

            else if (_type == typeof(Single))
                DbType = DbType.Single;

            else if (_type == typeof(TimeSpan))
                DbType = DbType.Time;

            else if (_type == typeof(UInt16))
                DbType = DbType.UInt16;

            else if (_type == typeof(UInt32))
                DbType = DbType.UInt32;

            else if (_type == typeof(UInt64))
                DbType = DbType.UInt64;

            else if (_type == typeof(XElement))
                DbType = DbType.Xml;
        }

        internal object GetValue(object item)
        {
            var value = _property.GetValue(item);
            return value;
        }

        internal object GetOldValue(object item)
        {
            var value = _property.GetValue(item);
            return value;
        }

        internal void SetValue(object item, object newValue)
        {
            _property.SetValue(item, newValue);
        }

        private PropertyInfo _property;
        private readonly List<ValidationAttribute> _validators;

    }


}