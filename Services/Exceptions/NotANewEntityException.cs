using refactor_me.Entities.Base;
using System;
using System.Runtime.Serialization;

namespace refactor_me.Services.Exceptions
{
    /// <summary>
    /// An exception thrown specifically when a caller tries to Create
    /// an entity which already exists
    /// </summary>
    [Serializable]
    public class NotANewEntityException : Exception
    {
        public BaseEntity ExceptionalEntity;

        public NotANewEntityException()
        {
        }

        public NotANewEntityException(string message) : base(message)
        {
        }

        public NotANewEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotANewEntityException(string message, BaseEntity product) : base(message)
        {
            this.ExceptionalEntity = product;
        }

        protected NotANewEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}