using System;

namespace Bb.OptionServer.Exceptions
{
    [Serializable]
    public class MissingUserException : Exception
    {
        public MissingUserException() { }
        public MissingUserException(string message) : base(message) { }
        public MissingUserException(string message, Exception inner) : base(message, inner) { }
        protected MissingUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
