using System;

namespace Bb.OptionServer
{
    internal class KeyItem
    {

        internal KeyItem(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }

        public object Value { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ (Value != null ? Value.GetHashCode() : DBNull.Value.GetHashCode());
        }


    }
}