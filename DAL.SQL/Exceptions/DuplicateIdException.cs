using System;
using System.Runtime.Serialization;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.Entities.Base;

namespace refactor_me.Dal.Sql.Repositories
{
    [Serializable]
    internal class DuplicateIdException : Exception
    {
        public BaseEntity Entity { get; private set; }
        public Guid EntityId { get; private set; }
        public string TableName { get; private set; }

        public DuplicateIdException()
        {
        }

        public DuplicateIdException(string message) : base(message)
        {
        }

        public DuplicateIdException(BaseEntity entity)
        {
            this.Entity = entity;
        }

        public DuplicateIdException(string tableName, Guid id)
        {
            this.TableName = tableName;
            this.EntityId = id;
        }

        public DuplicateIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}