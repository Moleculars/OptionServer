using Bb.OptionServer.Dao;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bb.OptionServer
{

    public class DtoSqlManager
    {


        public DtoSqlManager(SqlManager manager)
        {
            _generator = manager.QueryGenerator;
            _manager = manager;
        }


        public bool Insert<T>(T item)
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            mapping.Validate(item);

            var queryModel = ExtractQueryGenerator.Insert(item, mapping);
            QueryCommand query = _generator.Generate(queryModel);
            var result = _manager.Update(query.CommandText.ToString(), query.Arguments);

            if (result)
                item.Reset();

            return result;

        }

        public bool Update<T>(T item)
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            mapping.Validate(item);

            var queryModel = ExtractQueryGenerator.Update(item, mapping);
            QueryCommand query = _generator.Generate(queryModel);

            bool result = false;
            if (query != null && query.CommandText.Length > 0)
            {

                result = _manager.Update(query.CommandText.ToString(), query.Arguments);

                if (result)
                    item.Reset();

            }

            return result;

        }

        public bool Remove<T>(T item)
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            mapping.Validate(item);

            var queryModel = ExtractQueryGenerator.Insert(item, mapping);
            QueryCommand query = _generator.Generate(queryModel);

            bool result = false;
            if (query != null && query.CommandText.Length > 0)
            {

                result = _manager.Update(query.CommandText.ToString(), query.Arguments);

                if (result)
                    item.Reset();

            }

            return result;

        }


        public IEnumerable<T> Where<T>(params Expression<Func<T, bool>>[] e)
            where T : new()
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            var query = _generator.Select(mapping, e);

            var result = _manager.Read<T>(query.CommandText.ToString(), mapping, query.Arguments);

            return result;

        }

        public IEnumerable<T> Select<T>(params Expression<Func<T, bool>>[] e)
            where T : new()
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            var query = _generator.Select(mapping, e);

            var result = _manager.Read<T>(query.CommandText.ToString(), mapping, query.Arguments);

            return result;

        }


        public static ObjectMapping GetMapping(Type type)
        {
            if (!_mappings.TryGetValue(type, out ObjectMapping mapping))
                lock (_lock)
                    if (!_mappings.TryGetValue(type, out mapping))
                    {
                        _mappings.Add(type, mapping = new ObjectMapping(type));
                        mapping.Build();
                    }
            return mapping;

        }

        public SqlManager Sql => _manager;

        public IQueryGenerator Generator => _generator;

        private static readonly Dictionary<Type, ObjectMapping> _mappings = new Dictionary<Type, ObjectMapping>();
        private readonly IQueryGenerator _generator;
        private readonly SqlManager _manager;
        private static readonly object _lock = new object();

    }


}