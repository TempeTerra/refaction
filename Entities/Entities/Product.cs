using refactor_me.DomainObjects.Entities.Base;
using System;

namespace refactor_me.DomainObjects.Entities
{
    /// <summary>
    /// A high-level description of a product.
    /// 
    /// May have <see cref="ProductOption"/>s to specify different subtypes, 
    /// such as different colours.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// The name of the Product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the Product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the Product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The additional price to have the Product delivered
        /// </summary>
        public decimal DeliveryPrice { get; set; }

        /// <summary>
        /// Create a new Product
        /// </summary>
        /// <remarks>
        /// This will also get called by the model binder when data is
        /// sent to the controller over HTTP
        /// </remarks>
        public Product()
            : base(isNew: true)
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Retrieve an existing Product from storage
        /// </summary>
        /// <param name="id">The id of an existing stored product</param>
        public Product(Guid id)
            : base(isNew: false)
        {

        }
    }
}