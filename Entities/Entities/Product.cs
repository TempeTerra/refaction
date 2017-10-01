using refactor_me.DomainObjects.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace refactor_me.DomainObjects.Entities
{
    /// <summary>
    /// A high-level description of a product.
    /// 
    /// May have <see cref="ProductOption"/>s to specify different subtypes, 
    /// such as different colours.
    /// </summary>
    /// <remarks>
    /// -- Apparently validation of Decimals is a problem. Added a Range
    /// -- annotation from 0 to (18 significant figures) which should be
    /// -- right for the database columns (length 9, precision 18, scale 2)
    /// </remarks>
    public class Product : BaseEntity
    {
        /// <summary>
        /// The name of the Product
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// A description of the Product
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The price of the Product
        /// </summary>
        [Required]
        [Range(0, 9999999999999999.99)] // -- this needs a better solution
        public decimal Price { get; set; }

        /// <summary>
        /// The additional price to have the Product delivered
        /// </summary>
        [Required]
        [Range(0, 9999999999999999.99)] // -- this needs a better solution
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
        }

        /// <summary>
        /// Retrieve an existing Product from storage
        /// </summary>
        /// <param name="id">The id of an existing stored product</param>
        public Product(Guid id)
            : base(isNew: false)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{nameof(Product)} {Name}";
        }
    }
}