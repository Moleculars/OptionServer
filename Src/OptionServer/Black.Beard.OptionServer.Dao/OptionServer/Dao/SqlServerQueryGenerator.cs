using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Bb.OptionServer
{
    public class SqlServerQueryGenerator : IQueryGenerator
    {

        public SqlServerQueryGenerator(SqlManager manager)
        {
            Manager = manager;
            _predicateGenerator = new SqlServerPredicateGenerator();
            manager.QueryGenerator = this;
        }

        public SqlManager Manager { get; }

        private readonly IQueryPredicateGenerator _predicateGenerator;

        public (StringBuilder, DbParameter[]) Insert(object instance, ObjectMapping mapping)
        {

            StringBuilder sb = new StringBuilder(mapping.Columns * 100);

            sb.Append("INSERT INTO ");
            sb.Append(_predicateGenerator.WriteMember(mapping.TableName));

            sb.Append(" (");

            string comma = string.Empty;
            foreach (var fields in mapping.Fields)
            {
                sb.Append(comma);
                sb.Append(_predicateGenerator.WriteMember(fields.FieldName));
                comma = ", ";
            }

            sb.Append(") VALUES (");

            List<DbParameter> _arguments = new List<DbParameter>();
            comma = string.Empty;
            foreach (PropertyMapping field in mapping.Fields)
            {

                sb.Append(comma);

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
                        if (_f == null)
                        {
                            _f = new FieldValue<Guid>();
                            _f.Value = Guid.NewGuid();
                            field.SetValue(instance, _f);
                        }
                        if (Guid.Empty.Equals(_f.Value))
                        {
                            _f.Value = Guid.NewGuid();
                            //field.SetValue(instance, _f);
                        }
                    }

                }

                if (field.LastChangeDate)
                    sb.Append("CURRENT_TIMESTAMP");

                else
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

                    sb.Append(_predicateGenerator.WriteParameter(field.VariableName));
                    comma = ", ";
                    _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));
                }
            }

            sb.Append(")");

            return (sb, _arguments.ToArray());

        }

        public (StringBuilder, DbParameter[]) Update(object instance, ObjectMapping mapping)
        {

            StringBuilder sb = new StringBuilder(mapping.Columns * 100);
            List<DbParameter> _arguments = new List<DbParameter>();

            var c = instance.Changed().ToDictionary(c1 => c1.Item1);

            if (!c.Any())
                return (sb, _arguments.ToArray());

            sb.Append("UPDATE ");
            sb.Append(_predicateGenerator.WriteMember(mapping.TableName));
            sb.Append(" SET ");

            string comma = string.Empty;
            object newValueSecurity = null;
            PropertyMapping security = null;

            foreach (PropertyMapping field in mapping.Fields)
            {

                if (field.SecurityCoherence)
                {

                    newValueSecurity = field.GetValue(instance);
                    if (newValueSecurity is IField f)
                    {

                        newValueSecurity = f.Value;

                        if (!f.HasModified)
                        {

                            newValueSecurity = field.Type == typeof(Guid) || field.Type == typeof(FieldValue<Guid>)
                                ? (object)Guid.NewGuid()
                                : null
                                ;

                            f.Value = newValueSecurity;

                        }

                    }

                    sb.Append(comma);
                    sb.Append(_predicateGenerator.WriteMember(field.FieldName));
                    sb.Append(" = ");

                    sb.Append(_predicateGenerator.WriteParameter("new" + field.FieldName));
                    _arguments.Add(Manager.CreateParameter("new" + field.FieldName, field.DbType, newValueSecurity));
                    security = field;

                }
                else if (field.LastChangeDate)
                {

                    sb.Append(comma);
                    sb.Append(_predicateGenerator.WriteMember(field.FieldName));
                    sb.Append(" = ");

                    sb.Append("CURRENT_TIMESTAMP");
                }
                else
                {

                    if (!c.ContainsKey(field.Name))
                        continue;

                    sb.Append(comma);
                    sb.Append(_predicateGenerator.WriteMember(field.FieldName));
                    sb.Append(" = ");

                    var value = field.GetValue(instance);
                    if (value is IField f)
                        value = f.Value;

                    sb.Append(_predicateGenerator.WriteParameter(field.VariableName));
                    _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));
                }

                comma = ", ";

            }

            sb.Append(" WHERE ");
            comma = string.Empty;
            foreach (PropertyMapping field in mapping.Fields)
                if (field.IsPrimaryKey)
                {

                    sb.Append(comma);
                    sb.Append(_predicateGenerator.WriteMember(field.FieldName));
                    sb.Append(" = ");
                    sb.Append(_predicateGenerator.WriteParameter(field.VariableName));

                    var value = field.GetValue(instance);
                    if (value is IField f)
                    {
                        if (f.IsDefaultValue(f.OldValue))
                            value = f.Value;
                        else
                            value = f.OldValue;
                    }

                    _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));

                    comma = " AND ";

                }


            if (security != null)
            {

                var value = security.GetValue(instance);
                if (value is IField f)
                    value = f.OldValue;

                sb.Append(comma);
                sb.Append(security.FieldName);
                sb.Append(" = @");
                sb.Append("old" + security.FieldName);

                _arguments.Add(Manager.CreateParameter("old" + security.FieldName, security.DbType, value));

            }

            return (sb, _arguments.ToArray());

        }

        public (StringBuilder, DbParameter[]) Remove(object instance, ObjectMapping mapping)
        {

            StringBuilder sb = new StringBuilder(mapping.Columns * 100);
            List<DbParameter> _arguments = new List<DbParameter>();

            var c = instance.Changed().ToDictionary(c1 => c1.Item1);

            if (!c.Any())
                return (sb, _arguments.ToArray());

            sb.Append("DELETE FROM ");
            sb.Append(_predicateGenerator.WriteMember(mapping.TableName));

            string comma = string.Empty;

            sb.Append(" WHERE ");
            comma = string.Empty;
            foreach (PropertyMapping field in mapping.Fields)
                if (field.IsPrimaryKey)
                {

                    sb.Append(comma);
                    sb.Append(_predicateGenerator.WriteMember(field.FieldName));
                    sb.Append(" = ");
                    sb.Append(_predicateGenerator.WriteParameter(field.VariableName));

                    var value = field.GetValue(instance);
                    if (value is IField f)
                    {
                        if (f.IsDefaultValue(f.OldValue))
                            value = f.Value;
                        else
                            value = f.OldValue;
                    }

                    _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));

                    comma = " AND ";

                }

            foreach (PropertyMapping security in mapping.Fields)
                if (security.SecurityCoherence)
                {
                    var value = security.GetValue(instance);
                    if (value is IField f)
                        value = f.OldValue;

                    sb.Append(comma);
                    sb.Append(security.FieldName);
                    sb.Append(" = @");
                    sb.Append("old" + security.FieldName);

                    _arguments.Add(Manager.CreateParameter("old" + security.FieldName, security.DbType, value));
                }


            return (sb, _arguments.ToArray());

        }

        public (StringBuilder, DbParameter[]) Select<T>(ObjectMapping mapping, Expression<Func<T, bool>>[] e)
        {

            StringBuilder sb = new StringBuilder(mapping.Columns * 100);
            List<DbParameter> _arguments = new List<DbParameter>();

            sb.Append("SELECT ");
            string comma = string.Empty;
            foreach (PropertyMapping field in mapping.Fields)
            {

                sb.Append(comma);
                sb.Append(field.FieldName);

                comma = ", ";

            }

            sb.Append(" FROM ");
            sb.Append(mapping.TableName);
            sb.Append(" WHERE ");

            ParsePredicate(e.Cast<Expression>().ToArray(), sb, _arguments, mapping);

            return (sb, _arguments.ToArray());

        }

        private void ParsePredicate(Expression[] e, StringBuilder sb, List<DbParameter> arguments, ObjectMapping mapping)
        {

            var v = new PredicateVisitor(sb, arguments, _predicateGenerator, mapping, Manager);
            string comma = string.Empty;
            foreach (var item in e)
            {
                sb.Append(comma);
                v.Visit(item);
                comma = " AND";
            }

        }

        public string Declare(string variableName, DbType variableType, string value = null)
        {
            StringBuilder sb = new StringBuilder(200);

            sb.Append("DECLARE ");

            sb.Append(_predicateGenerator.WriteParameter(variableName));

            sb.Append(" AS ");

            sb.Append(_predicateGenerator.WriteType(variableType, value));

            // declare @a as varchar(10) = 'e'

            if (value != null)
            {
                sb.Append(" = ");
                sb.Append(value);
            }

            sb.Append(";");

            return sb.ToString();

        }

        public string GetValue(DbType dbType, object value)
        {

            switch (dbType)
            {

                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return _predicateGenerator.WriteStringValue(value);

                case DbType.Boolean:
                    return _predicateGenerator.WriteBooleanValue(value);

                case DbType.Guid:
                    return _predicateGenerator.WriteGuid(value);

                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                    return _predicateGenerator.WriteDatetime(value);

                case DbType.DateTimeOffset:
                    return _predicateGenerator.WriteTimeOffset(value);

                case DbType.Time:
                    return _predicateGenerator.WriteTime(value);

                case DbType.VarNumeric:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Single:
                    return _predicateGenerator.WriteDecimal(value);

                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                    return _predicateGenerator.WriteInt(value);

                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                    return _predicateGenerator.WriteUInt(value);

                case DbType.Object:
                case DbType.Binary:
                case DbType.Byte:
                case DbType.Currency:
                case DbType.SByte:
                case DbType.Xml:
                default:
                    // CultureInfo.CurrentCulture
                    return value.ToString();

            }
        }



        //        public StringBuilder Select2<T>(ObjectMapping mapping, Expression<Func<T, bool>>[] e)
        //{

        //    StringBuilder sb = new StringBuilder(mapping.Columns * 100);
        //    List<DbParameter> _arguments = new List<DbParameter>();

        //    sb.Append("SELECT ");
        //    string comma = string.Empty;
        //    foreach (PropertyMapping field in mapping.Fields)
        //    {

        //        sb.Append(comma);
        //        sb.Append(field.FieldName);

        //        comma = ", ";

        //    }

        //    sb.Append(" FROM ");
        //    sb.Append(mapping.TableName);

        //    return sb;

        //}
        //public (StringBuilder, DbParameter[]) Where<T>(StringBuilder sb, ObjectMapping mapping, Expression<Func<T, bool>>[] e)
        //{

        //    List<DbParameter> _arguments = new List<DbParameter>();

        //    sb.Append(" WHERE ");

        //    ParsePredicate(e.Cast<Expression>().ToArray(), sb, _arguments, mapping);

        //    return (sb, _arguments.ToArray());

        //}


    }

}