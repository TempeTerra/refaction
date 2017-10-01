using System;
using System.Runtime.Serialization;

namespace refactor_me.Dal.Sql.Repositories
{
    [Serializable]
    internal class DeleteIdNotFoundException : Exception
    {
        public Guid EntityId { get; private set; }
        public string TableName { get; private set; }

        public DeleteIdNotFoundException()
        {
        }

        public DeleteIdNotFoundException(string message) : base(message)
        {
        }

        public DeleteIdNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DeleteIdNotFoundException(string tableName, Guid id)
        {
            this.TableName = tableName;
            this.EntityId = id;
        }

        protected DeleteIdNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}