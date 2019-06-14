using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace Bb.OptionServer
{

    public interface IQueryGenerator
    {

        SqlManager Manager { get; }

        (StringBuilder, DbParameter[]) Insert(object instance, ObjectMapping mapping);

        (StringBuilder, DbParameter[]) Remove(object instance, ObjectMapping mapping);

        (StringBuilder, DbParameter[]) Update(object instance, ObjectMapping mapping);

        (StringBuilder, DbParameter[]) Select<T>(ObjectMapping mapping, Expression<Func<T, bool>>[] e);

        string Declare(string variableName, DbType variableType, string value);

        string GetValue(DbType dbType, object value);

    }

}