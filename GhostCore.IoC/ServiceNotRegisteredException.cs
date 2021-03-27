using System;
using System.Runtime.Serialization;

namespace GhostCore.IoC
{
    [Serializable]
    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException() { }
        public ServiceNotRegisteredException(string message) : base(message) { }
        public ServiceNotRegisteredException(string message, Exception inner) : base(message, inner) { }
        protected ServiceNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
