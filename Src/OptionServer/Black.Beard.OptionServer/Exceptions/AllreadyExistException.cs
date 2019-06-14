using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.OptionServer.Exceptions
{

    [Serializable]
    public class AllreadyExistException : Exception
    {
        public AllreadyExistException() { }
        public AllreadyExistException(string message) : base(message) { }
        public AllreadyExistException(string message, Exception inner) : base(message, inner) { }
        protected AllreadyExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
