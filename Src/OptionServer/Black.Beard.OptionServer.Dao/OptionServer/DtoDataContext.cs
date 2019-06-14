//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Bb.OptionServer
//{

//    public class DtoDataContext
//    {

//        public DtoDataContext(DtoSqlManager dtoManager)
//        {
//            this.dtoManager = dtoManager;
//        }



//        private class Datas
//        {

//            public Datas(Type type)
//            {

//                this.Type = type;
//                this.Mapping = DtoSqlManager.GetMapping(type);

//                var keys = this.Mapping.Keys;
//                this._datas = new Dictionary<KeyMapping, Box>();

//            }

//            public Type Type { get; }

//            public ObjectMapping Mapping { get; }

//            internal void Insert<T>(T[] items)
//            {
//                foreach (var item in items)
//                {
//                    KeyMapping key = this.Mapping.GetKey(item);
//                    this._datas.Add(key, new Box(key, item) { Insert = true });
//                }
//            }

//            private class Box
//            {

//                public Box(KeyMapping key, object item)
//                {
//                    this.Key = key;
//                    this.Item = item;
//                }

//                public bool Insert { get; set; }

//                public KeyMapping Key { get; }

//                public object Item { get; }

//            }

//            internal IEnumerable<(bool, object)> Items()
//            {

//                foreach (Box item in _datas)
//                {

//                    if (item.Insert)
//                        yield return (true, item.Item);

//                    //item.Item.IsChanged

//                }

//            }

//            private readonly Dictionary<KeyMapping, Box> _datas;

//        }

//        private Dictionary<Type, Datas> _items = new Dictionary<Type, Datas>();
//        private readonly DtoSqlManager dtoManager;

//        public void Insert<T>(params T[] items)
//        {

//            var type = typeof(T);
//            if (!_items.TryGetValue(type, out Datas box))
//                _items.Add(type, box = new Datas(type));

//            box.Insert(items);

//        }

//        public void Save()
//        {

//            foreach (Datas datas in this._items)
//            {
//                datas.items
//            }

//            using (this.dtoManager.Manager.GetTransaction())
//            {

//            }

//        }
//    }


//}
