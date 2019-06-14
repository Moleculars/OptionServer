using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.OptionServer.Exceptions
{

    [Serializable]
    public class InvalidNameException : Exception
    {
        public InvalidNameException() { }
        public InvalidNameException(string message) : base(message) { }
        public InvalidNameException(string message, Exception inner) : base(message, inner) { }
        protected InvalidNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
