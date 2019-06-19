using Bb.OptionServer.Dao;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Bb.OptionServer
{
    public class ExtractQueryGenerator
    {

        public static QuerySqlCommand Insert(object instance, ObjectMapping mapping)
        {

            var result = new QuerySqlCommand()
            {
                Kind = QueryKindEnum.Insert,
                Table = mapping.TableName,
            };

            List<DbParameter> _arguments = new List<DbParameter>();
            foreach (PropertyMapping field in mapping.Fields)
            {

                var _field = new Field()
                {
                    Name = field.FieldName,
                    VariableName = field.VariableName,
                    DbType = field.DbType,
                    Value = new FieldValue()
                };

                result.Fields.Add(_field);

                if (field.SecurityCoherence)
                {

                    if (field.Type == typeof(Guid))
                    {
                        if (Guid.Empty.Equals(field.GetValue(instance)))
                            field.SetValue(instance, Guid.NewGuid());
                    }
                    else if (field.Type == typeof(FieldValue<Guid>))
                    {
                        var _f = field.GetValue(instance) as IField;
                        if (Guid.Empty.Equals(_f.Value))
                            _f.Value = Guid.NewGuid();
                    }

                }

                if (field.LastChangeDate)
                    _field.Value = FieldValue.CURRENT_TIMESTAMP;

                else
                    _field.Value.Data = ReadValue(instance, field);

            }

            return result;

        }

        public static QuerySqlCommand Update(object instance, ObjectMapping mapping)
        {

            QuerySqlCommand result = new QuerySqlCommand()
            {
                Kind = QueryKindEnum.Update,
                Table = mapping.TableName
            };

            List<Field> _fields = new List<Field>();

            var c = instance.Changed().ToDictionary(c1 => c1.Item1);

            foreach (PropertyMapping field in mapping.Fields)
            {

                var _field = new Field()
                {
                    Name = field.Name,
                    VariableName = field.VariableName,
                    DbType = field.DbType,
                    Value = new FieldValue(),
                };

                var value = field.GetValue(instance);
                IField f1 = value as IField;

                if (f1 != null)
                {
                    _field.Value.Data = f1.Value;
                    _field.Value.OldData = f1.OldValue;
                }
                else
                    _field.Value.OldData = _field.Value.Data = value;

                if (field.IsPrimaryKey)
                {

                    if (field.IsGuid() && f1 != null && (Guid)f1.Value == Guid.Empty)
                    {
                        _field.Value.Data = f1.Value = Guid.NewGuid();
                        _field.Value.OldData = f1.OldValue;
                    }

                    result.FilterFields.Add(new Field()
                    {
                        Name = _field.Name,
                        DbType = field.DbType,
                        VariableName = field.VariableName,
                        Value = new FieldValue()
                        {
                            Data = _field.Value.OldData,
                        }
                    });

                    _field.VariableName = f1.HasModified ? "new" + field.FieldName : field.VariableName;

                    if (f1.HasModified)
                        _fields.Add(_field);

                }
                if (field.SecurityCoherence)
                {

                    if (field.IsGuid() && f1 != null && !f1.HasModified)
                    {
                        _field.Value.Data = f1.Value = Guid.NewGuid();
                        _field.Value.OldData = f1.OldValue;
                    }

                    result.FilterFields.Add(new Field()
                    {
                        Name = _field.Name,
                        DbType = field.DbType,
                        VariableName = field.VariableName,
                        Value = new FieldValue()
                        {
                            Data = _field.Value.OldData,
                        }
                    });

                    _field.VariableName = f1.HasModified ? "new" + field.FieldName : field.VariableName;
                    _fields.Add(_field);

                }
                else if (field.LastChangeDate)
                {
                    _field.Value = FieldValue.CURRENT_TIMESTAMP;
                    _fields.Add(_field);
                }

                else if (c.ContainsKey(field.Name))
                    result.Fields.Add(_field);

            }

            if (result.Fields.Count > 0)
                result.Fields.AddRange(_fields);

            return result;

        }

        public static QuerySqlCommand Remove(object instance, ObjectMapping mapping)
        {

            QuerySqlCommand result = new QuerySqlCommand()
            {
                Kind = QueryKindEnum.Delete,
                Table = mapping.TableName,
            };

            foreach (PropertyMapping field in mapping.Fields)
            {

                if (field.IsPrimaryKey)
                {

                    Field _field = new Field()
                    {
                        Name = field.Name,
                        VariableName = field.VariableName,
                        DbType = field.DbType,
                        Value = new FieldValue(),
                    };

                    var value = field.GetValue(instance);
                    if (value is IField f)
                    {
                        if (f.IsDefaultValue(f.OldValue))
                            _field.Value.Data = f.Value;
                        else
                            _field.Value.Data = f.OldValue;
                    }

                    result.FilterFields.Add(_field);

                }
                else if (field.SecurityCoherence)
                {

                    Field _field = new Field()
                    {
                        Name = field.Name,
                        DbType = field.DbType,
                        VariableName = field.VariableName,
                        Value = new FieldValue(),
                    };

                    var value = field.GetValue(instance);
                    if (value is IField f)
                        _field.Value.Data = f.OldValue;

                    result.FilterFields.Add(_field);

                }

            }

            return result;

        }

        private static object ReadValue(object instance, PropertyMapping field)
        {
            var value = field.GetValue(instance);
            if (value == null)
                value = DBNull.Value;

            else if (value is IField f)
            {
                if (f == null)
                    value = DBNull.Value;
                else
                    value = f.Value;
            }

            return value;
        }


    }

}