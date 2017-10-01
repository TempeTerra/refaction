using System;
using System.Runtime.Serialization;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.Entities.Base;

namespace refactor_me.Dal.Sql.Repositories
{
    [Serializable]
    internal class NoRowsUpdatedException : Exception
    {
        public BaseEntity Entity;

        public NoRowsUpdatedException()
        {
        }

        public NoRowsUpdatedException(string message) : base(message)
        {
        }

        public NoRowsUpdatedException(BaseEntity entity)
        {
            this.Entity = entity;
        }

        public NoRowsUpdatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoRowsUpdatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}