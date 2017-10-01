using System;
using System.Runtime.Serialization;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.Entities.Base;

namespace refactor_me.Dal.Sql.Repositories
{
    [Serializable]
    internal class NoRowsCreatedException : Exception
    {
        public BaseEntity Entity;

        public NoRowsCreatedException()
        {
        }

        public NoRowsCreatedException(string message) : base(message)
        {
        }

        public NoRowsCreatedException(BaseEntity entity)
        {
            this.Entity = entity;
        }

        public NoRowsCreatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoRowsCreatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}