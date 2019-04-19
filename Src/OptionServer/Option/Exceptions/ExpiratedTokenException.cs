using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Option.Exceptions
{


    [Serializable]
    public class ExpiratedTokenException : Exception
    {
        public ExpiratedTokenException() { }
        public ExpiratedTokenException(string message) : base(message) { }
        public ExpiratedTokenException(string message, Exception inner) : base(message, inner) { }
        protected ExpiratedTokenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
