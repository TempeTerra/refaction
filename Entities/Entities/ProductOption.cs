using Newtonsoft.Json;
using refactor_me.DomainObjects.Entities.Base;
using System;

namespace refactor_me.DomainObjects.Entities
{
    /// <summary>
    /// An optional subtype of a <see cref="Product"/>, for example a colour variant
    /// </summary>
    public class ProductOption : BaseEntity
    {
        /// <summary>
        /// The ID of the product this option applies to
        /// </summary>
        [JsonIgnore]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The full product name including the option variant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Create a new ProductOption
        /// </summary>
        /// <remarks>
        /// This will also get called by the model binder when data is
        /// sent to the controller over HTTP
        /// </remarks>
        public ProductOption()
            : base(isNew: true)
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Retrieve an existing ProductOption from storage
        /// </summary>
        /// <param name="id">The id of an existing stored ProductOption</param>
        public ProductOption(Guid id)
            : base(isNew: false)
        {

        }
    }
}