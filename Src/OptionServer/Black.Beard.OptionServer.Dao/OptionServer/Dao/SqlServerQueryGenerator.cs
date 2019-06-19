using Bb.OptionServer.Dao;
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
            _predicateGenerator = SqlServerPredicateGenerator.Instance;
        }

        public SqlManager Manager { get; }

        private readonly IQueryPredicateGenerator _predicateGenerator;

        #region auto

        public QueryCommand Generate(QuerySqlCommand query)
        {

            if (query.Kind == QueryKindEnum.Insert)
                return Insert(query);

            if (query.Kind == QueryKindEnum.Update)
                return Update(query);

            if (query.Kind == QueryKindEnum.Delete)
                return Remove(query);

            return null;

        }

        public QueryCommand Update(QuerySqlCommand query)
        {

            StringBuilder sb = new StringBuilder(query.Fields.Count * 100);
            List<DbParameter> _arguments = new List<DbParameter>();

            sb.Append("UPDATE ");
            sb.Append(_predicateGenerator.WriteMember(query.Table));
            sb.Append(" SET ");

            string comma = string.Empty;

            foreach (Field field in query.Fields)
            {

                sb.Append(comma);
                sb.Append(_predicateGenerator.WriteMember(field.Name));
                sb.Append(" = ");

                if (field.Value == FieldValue.CURRENT_TIMESTAMP)
                    sb.Append(_predicateGenerator.CurrentTimestamp);

                else
                {
                    var value = field.Value.Data;
                    sb.Append(_predicateGenerator.WriteParameter(field.VariableName));
                    _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));
                }

                comma = ", ";

            }

            sb.Append(" WHERE ");
            comma = string.Empty;
            foreach (Field field in query.FilterFields)
            {

                sb.Append(comma);
                sb.Append(_predicateGenerator.WriteMember(field.Name));
                sb.Append(" = ");
                sb.Append(_predicateGenerator.WriteParameter(field.VariableName));

                var value = field.Value.Data;
                _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));

                comma = " AND ";

            }

            return new QueryCommand(sb, _arguments.ToArray());

        }

        public QueryCommand Remove(QuerySqlCommand query)
        {

            StringBuilder sb = new StringBuilder(query.Fields.Count * 100);
            List<DbParameter> _arguments = new List<DbParameter>();

            sb.Append("DELETE FROM ");
            sb.Append(_predicateGenerator.WriteMember(query.Table));

            string comma = string.Empty;

            sb.Append(" WHERE ");
            comma = string.Empty;
            foreach (Field field in query.FilterFields)
            {

                sb.Append(comma);
                sb.Append(_predicateGenerator.WriteMember(field.Name));
                sb.Append(" = ");
                sb.Append(_predicateGenerator.WriteParameter(field.VariableName));

                var value = field.Value;
                _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));

                comma = " AND ";

            }

            return new QueryCommand(sb, _arguments.ToArray());

        }

        private QueryCommand Insert(QuerySqlCommand query)
        {

            StringBuilder sbSql1 = new StringBuilder(query.Fields.Count * 100);

            sbSql1.Append("INSERT INTO ");
            sbSql1.Append(_predicateGenerator.WriteMember(query.Table));
            sbSql1.Append(" (");

            string comma = string.Empty;
            foreach (var field in query.Fields)
            {
                sbSql1.Append(comma);
                sbSql1.Append(_predicateGenerator.WriteMember(field.Name));
                comma = ", ";
            }

            sbSql1.Append(") VALUES (");

            List<DbParameter> _arguments = new List<DbParameter>();
            comma = string.Empty;
            foreach (Field field in query.Fields)
            {

                sbSql1.Append(comma);

                if (field.Value == FieldValue.CURRENT_TIMESTAMP)
                    sbSql1.Append(_predicateGenerator.CurrentTimestamp);

                else
                {

                    var value = field.Value.Data;
                    if (value == null)
                        value = DBNull.Value;

                    sbSql1.Append(_predicateGenerator.WriteParameter(field.VariableName));
                    comma = ", ";
                    _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));
                }
            }

            sbSql1.Append(")");

            return new QueryCommand(sbSql1, _arguments.ToArray());

        }

        #endregion auto

        public QueryCommand Select<T>(ObjectMapping mapping, Expression<Func<T, bool>>[] e)
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

            return new QueryCommand(sb, _arguments.ToArray());

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



        
        //#region manuel

        //public QueryCommand Insert(object instance, ObjectMapping mapping)
        //{

        //    StringBuilder sbSql1 = new StringBuilder(mapping.Columns * 100);

        //    sbSql1.Append("INSERT INTO ");
        //    sbSql1.Append(_predicateGenerator.WriteMember(mapping.TableName));
        //    sbSql1.Append(" (");

        //    string comma = string.Empty;
        //    foreach (var fields in mapping.Fields)
        //    {
        //        sbSql1.Append(comma);
        //        sbSql1.Append(_predicateGenerator.WriteMember(fields.FieldName));
        //        comma = ", ";
        //    }

        //    sbSql1.Append(") VALUES (");

        //    List<DbParameter> _arguments = new List<DbParameter>();
        //    comma = string.Empty;
        //    foreach (PropertyMapping field in mapping.Fields)
        //    {

        //        sbSql1.Append(comma);

        //        if (field.SecurityCoherence)
        //        {

        //            if (field.Type == typeof(Guid))
        //            {
        //                if (Guid.Empty.Equals(field.GetValue(instance)))
        //                    field.SetValue(instance, Guid.NewGuid());
        //            }
        //            else if (field.Type == typeof(FieldValue<Guid>))
        //            {
        //                var _f = field.GetValue(instance) as IField;
        //                if (_f == null)
        //                {
        //                    _f = new FieldValue<Guid>();
        //                    _f.Value = Guid.NewGuid();
        //                    field.SetValue(instance, _f);
        //                }
        //                if (Guid.Empty.Equals(_f.Value))
        //                {
        //                    _f.Value = Guid.NewGuid();
        //                    //field.SetValue(instance, _f);
        //                }
        //            }

        //        }

        //        if (field.LastChangeDate)
        //            sbSql1.Append("CURRENT_TIMESTAMP");

        //        else
        //        {
        //            var value = field.GetValue(instance);
        //            if (value == null)
        //                value = DBNull.Value;

        //            else if (value is IField f)
        //            {
        //                if (f == null)
        //                    value = DBNull.Value;
        //                else
        //                    value = f.Value;
        //            }

        //            sbSql1.Append(_predicateGenerator.WriteParameter(field.VariableName));
        //            comma = ", ";
        //            _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));
        //        }
        //    }

        //    sbSql1.Append(")");

        //    return new QueryCommand(sbSql1, _arguments.ToArray());

        //}

        //public QueryCommand Update(object instance, ObjectMapping mapping)
        //{

        //    StringBuilder sb = new StringBuilder(mapping.Columns * 100);
        //    List<DbParameter> _arguments = new List<DbParameter>();

        //    var c = instance.Changed().ToDictionary(c1 => c1.Item1);

        //    if (!c.Any())
        //        return null;

        //    sb.Append("UPDATE ");
        //    sb.Append(_predicateGenerator.WriteMember(mapping.TableName));
        //    sb.Append(" SET ");

        //    string comma = string.Empty;
        //    object newValueSecurity = null;
        //    PropertyMapping security = null;

        //    foreach (PropertyMapping field in mapping.Fields)
        //    {

        //        if (field.SecurityCoherence)
        //        {

        //            newValueSecurity = field.GetValue(instance);
        //            if (newValueSecurity is IField f)
        //            {

        //                newValueSecurity = f.Value;

        //                if (!f.HasModified)
        //                {

        //                    newValueSecurity = field.Type == typeof(Guid) || field.Type == typeof(FieldValue<Guid>)
        //                        ? (object)Guid.NewGuid()
        //                        : null
        //                        ;

        //                    f.Value = newValueSecurity;

        //                }

        //            }

        //            sb.Append(comma);
        //            sb.Append(_predicateGenerator.WriteMember(field.FieldName));
        //            sb.Append(" = ");

        //            sb.Append(_predicateGenerator.WriteParameter("new" + field.FieldName));
        //            _arguments.Add(Manager.CreateParameter("new" + field.FieldName, field.DbType, newValueSecurity));
        //            security = field;

        //        }
        //        else if (field.LastChangeDate)
        //        {

        //            sb.Append(comma);
        //            sb.Append(_predicateGenerator.WriteMember(field.FieldName));
        //            sb.Append(" = ");

        //            sb.Append(_predicateGenerator.CurrentTimestamp);

        //        }
        //        else
        //        {

        //            if (!c.ContainsKey(field.Name))
        //                continue;

        //            sb.Append(comma);
        //            sb.Append(_predicateGenerator.WriteMember(field.FieldName));
        //            sb.Append(" = ");

        //            var value = field.GetValue(instance);
        //            if (value is IField f)
        //                value = f.Value;

        //            sb.Append(_predicateGenerator.WriteParameter(field.VariableName));
        //            _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));
        //        }

        //        comma = ", ";

        //    }

        //    sb.Append(" WHERE ");
        //    comma = string.Empty;
        //    foreach (PropertyMapping field in mapping.Fields)
        //        if (field.IsPrimaryKey)
        //        {

        //            sb.Append(comma);
        //            sb.Append(_predicateGenerator.WriteMember(field.FieldName));
        //            sb.Append(" = ");
        //            sb.Append(_predicateGenerator.WriteParameter(field.VariableName));

        //            var value = field.GetValue(instance);
        //            if (value is IField f)
        //            {
        //                if (f.IsDefaultValue(f.OldValue))
        //                    value = f.Value;
        //                else
        //                    value = f.OldValue;
        //            }

        //            _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));

        //            comma = " AND ";

        //        }


        //    if (security != null)
        //    {

        //        var value = security.GetValue(instance);
        //        if (value is IField f)
        //            value = f.OldValue;

        //        sb.Append(comma);
        //        sb.Append(security.FieldName);
        //        sb.Append(" = @");
        //        sb.Append("old" + security.FieldName);

        //        _arguments.Add(Manager.CreateParameter("old" + security.FieldName, security.DbType, value));

        //    }

        //    return new QueryCommand(sb, _arguments.ToArray());

        //}

        //public QueryCommand Remove(object instance, ObjectMapping mapping)
        //{

        //    StringBuilder sb = new StringBuilder(mapping.Columns * 100);
        //    List<DbParameter> _arguments = new List<DbParameter>();

        //    sb.Append("DELETE FROM ");
        //    sb.Append(_predicateGenerator.WriteMember(mapping.TableName));

        //    string comma = string.Empty;

        //    sb.Append(" WHERE ");
        //    comma = string.Empty;
        //    foreach (PropertyMapping field in mapping.Fields)
        //        if (field.IsPrimaryKey)
        //        {

        //            sb.Append(comma);
        //            sb.Append(_predicateGenerator.WriteMember(field.FieldName));
        //            sb.Append(" = ");
        //            sb.Append(_predicateGenerator.WriteParameter(field.VariableName));

        //            var value = field.GetValue(instance);
        //            if (value is IField f)
        //            {
        //                if (f.IsDefaultValue(f.OldValue))
        //                    value = f.Value;
        //                else
        //                    value = f.OldValue;
        //            }

        //            _arguments.Add(Manager.CreateParameter(field.VariableName, field.DbType, value));

        //            comma = " AND ";

        //        }

        //    foreach (PropertyMapping security in mapping.Fields)
        //        if (security.SecurityCoherence)
        //        {
        //            var value = security.GetValue(instance);
        //            if (value is IField f)
        //                value = f.OldValue;

        //            sb.Append(comma);
        //            sb.Append(security.FieldName);
        //            sb.Append(" = @");
        //            sb.Append("old" + security.FieldName);

        //            _arguments.Add(Manager.CreateParameter("old" + security.FieldName, security.DbType, value));
        //        }


        //    return new QueryCommand(sb, _arguments.ToArray());

        //}

        //#endregion manuel

    }

}