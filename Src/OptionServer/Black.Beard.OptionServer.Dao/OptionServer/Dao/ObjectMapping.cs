using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Bb.OptionServer
{
    public class ObjectMapping
    {
        private readonly Dictionary<string, PropertyMapping> _properties;

        internal ObjectMapping(Type type)
        {
            Type = type;
            _properties = new Dictionary<string, PropertyMapping>();
            TableName = type.Name;
        }

        public Type Type { get; }
        public int Columns => _properties.Count;

        public string TableName { get; private set; }

        public IEnumerable<PropertyMapping> Fields => _properties.Values;


        public List<PropertyMapping> Keys => _keys ?? (_keys = _properties.Values.Where(c => c.IsPrimaryKey).OrderBy(c => c.FieldName).ToList());

        private List<PropertyMapping> _keys;
        private Dictionary<string, PropertyMapping> _indexByFieldName;
        private Dictionary<string, PropertyMapping> _indexByName;

        internal KeyMapping GetKey<T>(T item)
        {

            var key = new KeyMapping();

            foreach (var k in Keys)
                key.Add(new KeyItem(k.Name, k.GetValue(item)));

            return key;

        }

        public Dictionary<string, PropertyMapping> IndexByFieldName
        {
            get
            {
                if (_indexByFieldName == null)
                    _indexByFieldName = _properties.Values.ToDictionary(c => c.FieldName);
                return _indexByFieldName;
            }
        }

        public Dictionary<string, PropertyMapping> IndexByName
        {
            get
            {
                if (_indexByName == null)
                    _indexByName = _properties.Values.ToDictionary(c => c.Name);
                return _indexByName;
            }
        }

        public void Validate(object instance)
        {

            foreach (var item in _properties)
                item.Value.Validate(instance);

        }

        internal void Build()
        {

            var attributes = Type.GetCustomAttributes();
            foreach (Attribute attribute in attributes)
            {

                if (attribute is TableAttribute t)
                {
                    TableName = t.Name;
                    if (!string.IsNullOrEmpty(t.Schema))
                        TableName = t.Schema + "." + TableName;
                }

            }

            var items = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var item in items)
            {
                PropertyMapping propertyMapping = new PropertyMapping(item);
                propertyMapping.Build();
                _properties.Add(propertyMapping.Name, propertyMapping);
            }

        }

        internal void Map<T>(DbDataReaderContext ctx, T result)
        {

            for (int i = 0; i < ctx.Reader.FieldCount; i++)
            {

                var datas = ctx.GetValue(i);
                if (IndexByFieldName.TryGetValue(datas.Item1, out PropertyMapping mapping))
                {
                    if (datas.Item2 != DBNull.Value)
                    {
                        if (mapping.Type != datas.Item2.GetType())
                        {
                            var t2 = typeof(FieldValue<>).MakeGenericType(datas.Item2.GetType());
                            if (mapping.Type == t2)
                            {
                                var value = mapping.GetValue(result) as IField;
                                if (value == null)
                                {
                                    value = Activator.CreateInstance(mapping.Type) as IField;
                                    mapping.SetValue(result, value);
                                }

                                value.Value = datas.Item2;
                            }

                        }
                        else
                            mapping.SetValue(result, datas.Item2);

                    }
                }
                else
                    throw new MissingMemberException(datas.Item1);

            }

        }

    }


}


