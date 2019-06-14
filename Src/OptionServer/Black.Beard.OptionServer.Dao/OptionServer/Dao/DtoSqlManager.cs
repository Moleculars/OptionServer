using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bb.OptionServer
{

    public class DtoSqlManager
    {

        public DtoSqlManager(IQueryGenerator generator)
        {
            _generator = generator;
            _manager = generator.Manager;
        }

        public bool Insert<T>(T item)
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            mapping.Validate(item);

            var query = _generator.Insert(item, mapping);

            var result = _manager.Update(query.Item1.ToString(), query.Item2);

            if (result)
                item.Reset();

            return result;

        }

        public bool Update<T>(T item)
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            mapping.Validate(item);

            var query = _generator.Update(item, mapping);

            bool result = false;
            if (query.Item1.Length > 0)
            {

                result = _manager.Update(query.Item1.ToString(), query.Item2);

                if (result)
                    item.Reset();

            }

            return result;

        }

        public bool Remove<T>(T item)
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            mapping.Validate(item);

            var query = _generator.Remove(item, mapping);

            bool result = false;
            if (query.Item1.Length > 0)
            {

                result = _manager.Update(query.Item1.ToString(), query.Item2);

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

            var result = _manager.Read<T>(query.Item1.ToString(), mapping, query.Item2);

            return result;

        }

        public IEnumerable<T> Select<T>(params Expression<Func<T, bool>>[] e)
            where T : new()
        {

            ObjectMapping mapping = GetMapping(typeof(T));

            var query = _generator.Select(mapping, e);

            var result = _manager.Read<T>(query.Item1.ToString(), mapping, query.Item2);

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

        private static readonly Dictionary<Type, ObjectMapping> _mappings = new Dictionary<Type, ObjectMapping>();
        private readonly IQueryGenerator _generator;
        private readonly SqlManager _manager;
        private static readonly object _lock = new object();

    }


}