using Newtonsoft.Json;
using System;

namespace refactor_me.DomainObjects.Entities.Base
{
    /// <summary>
    /// A base class for other models in this project.
    /// </summary>
    /// <remarks>
    /// Captures the common aspects of database-backed models
    /// - Primary key / Id
    /// - Flag for IsNew
    /// 
    /// Enforces correct setting of IsNew from subclasses.
    /// Checks for default-value primary keys, which sounds like an error.
    /// </remarks>
    public class BaseEntity
    {
        private Guid _id;

        /// <summary>
        /// The primary key of this object
        /// </summary>
        /// <remarks>
        /// -- I'm assuming that this should always have a value on a properly
        /// -- constructed object - either retrieved from the database or
        /// -- a new GUID if it's a new object.
        /// </remarks>
        public Guid Id
        {
            get
            {
                return _id;
            }

            set
            {
                if(value == default(Guid))
                {
                    throw new ArgumentException("Models may not be assigned a default ID");
                }

                _id = value;
            }
        }

        /// <summary>
        /// Indicates that the object is new rather than loaded from storage
        /// </summary>
        [JsonIgnore]
        public bool IsNew { get; } = true;

        /// <summary>
        /// Protected constructor available to subclasses. The parameter
        /// ensures the subclass is clear about whether it's a new object
        /// or one loaded from storage.
        /// </summary>
        /// <param name="isNew">False if the object already exists in storage</param>
        protected BaseEntity(bool isNew)
        {
            IsNew = isNew;
        }
    }
}