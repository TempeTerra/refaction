using System;
using System.Runtime.Serialization;

namespace refactor_me.Services.Exceptions
{
    /// <summary>
    /// An exception thrown when a requestion product option doesn't match
    /// the right product
    /// </summary>
    /// <remarks>
    /// For example, Rose Gold for Galaxy S7 which is not a valid combination
    /// of Product and ProductOption
    /// </remarks>
    [Serializable]
    internal class ProductOptionMismatchException : Exception
    {
        public ProductOptionMismatchException()
        {
        }

        public ProductOptionMismatchException(string message) : base(message)
        {
        }

        public ProductOptionMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductOptionMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}