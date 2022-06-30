using System;
using System.Runtime.Serialization;

namespace GhostCore.IoC
{
    [Serializable]
    public class MockProviderMissingException : Exception
    {
        public MockProviderMissingException() { }
        public MockProviderMissingException(string message) : base(message) { }
        public MockProviderMissingException(string message, Exception inner) : base(message, inner) { }
        protected MockProviderMissingException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}
