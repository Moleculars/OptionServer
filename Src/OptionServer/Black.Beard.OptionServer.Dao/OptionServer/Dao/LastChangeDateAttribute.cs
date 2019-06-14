using System;

namespace Bb.OptionServer
{

    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class LastChangeDateAttribute : Attribute
    {

        // This is a positional argument
        public LastChangeDateAttribute()
        {

        }

    }

}
