using System;

namespace Bb.Exceptions
{
    [Serializable]
    public class NotEnoughtRightException : Exception
    {
        public NotEnoughtRightException() { }
        public NotEnoughtRightException(string message) : base(message) { }
        public NotEnoughtRightException(string message, Exception inner) : base(message, inner) { }
        protected NotEnoughtRightException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
