using System;
using System.Runtime.Serialization;

namespace refactor_me.Services.Exceptions
{
    /// <summary>
    /// A general exception thrown from the refactor-me service layer
    /// when a service call fails. See the inner exception for more specific
    /// information.
    /// </summary>
    /// <remarks>
    /// This exception should be thrown when a service call failed in a
    /// dependency rather than when the service itself rejects a call because
    /// of, for example, data validation failure.
    /// </remarks>
    [Serializable]
    public class OperationFailedException : Exception
    {
        public OperationFailedException()
        {
        }

        public OperationFailedException(string message) : base(message)
        {
        }

        public OperationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}