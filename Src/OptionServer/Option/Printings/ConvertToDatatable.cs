using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Option.Printings
{

    public static class ConvertToDatatable
    {

        public static DataTable Convert<T>(T result, string label, params Expression<Func<T, object>>[] items)
        {
            return ConvertList(new T[] { result }, label, items);
        }

        public static DataTable ConvertList<T>(IEnumerable<T> rows, string label, params Expression<Func<T, object>>[] columns)
        {

            List<PropertyInfo> members = (columns.Length > 0)
                ? GetMember(columns).ToList()
                : typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();

            DataTable table = new DataTable
            {
                TableName = label ?? typeof(T).Name,
            };

            foreach (var item in members)
            {
                Type type = item.PropertyType;
                var column = table.Columns.Add(" " + item.Name.Replace("_", " ") + " ", type);
            }

            foreach (var rowSource in rows)
            {

                List<object> rowTarget = new List<object>();

                foreach (var item in members)
                    rowTarget.Add(item.GetValue(rowSource));

                table.Rows.Add(rowTarget.ToArray());

            }

            return table;

        }

        public static IEnumerable<PropertyInfo> GetMember<T>(params Expression<Func<T, object>>[] items)
        {

            foreach (var item in items)
                yield return Visitor.Get(item);

        }

        private class Visitor : System.Linq.Expressions.ExpressionVisitor
        {

            public static PropertyInfo Get(Expression e)
            {
                var v = new Visitor();
                v.Visit(e);
                return v._member;
            }

            protected override Expression VisitMember(MemberExpression node)
            {

                _member = node.Member as PropertyInfo;

                return base.VisitMember(node);
            }

            private PropertyInfo _member;

        }

    }

}
