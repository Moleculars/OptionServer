using System;

namespace Bb.OptionServer.Exceptions
{
    [Serializable]
    public class AmbigiousNameException : Exception
    {
        public AmbigiousNameException() { }
        public AmbigiousNameException(string message) : base(message) { }
        public AmbigiousNameException(string message, Exception inner) : base(message, inner) { }
        protected AmbigiousNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
