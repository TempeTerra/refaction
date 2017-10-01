using refactor_me.DomainObjects.Entities;
using System.Collections.Generic;
using System.Linq;

namespace refactor_me.DomainObjects.ValueTypes
{
    /// <summary>
    /// Represents a list of <see cref="ProductOption"/>s
    /// </summary>
    public class ProductOptions
    {
        /// <summary>
        /// The list of items
        /// </summary>
        /// <remarks>
        /// This is just to make sure the JSON renders as expected
        /// </remarks>
        public IEnumerable<ProductOption> Items { get; }

        /// <summary>
        /// Get a list of all the <see cref="ProductOption"/>s
        /// </summary>
        public ProductOptions(IEnumerable<ProductOption> items)
        {
            Items = items;
        }

        public override string ToString()
        {
            return $"{nameof(ProductOptions)}[{Items?.Count()}]";
        }
    }
}