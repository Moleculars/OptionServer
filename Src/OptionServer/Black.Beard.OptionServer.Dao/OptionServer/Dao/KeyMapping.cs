using System;
using System.Collections.Generic;

namespace Bb.OptionServer
{

    public class KeyMapping
    {

        public KeyMapping()
        {
            this._list = new List<KeyItem>();
        }


        internal void Add(KeyItem keyItem)
        {

            _list.Add(keyItem);

        }

        public override int GetHashCode()
        {

            int value = 0;

            foreach (var item in _list)
                value ^= GetHashCode();

            return value;

        }

        private List<KeyItem> _list;

    }


}


