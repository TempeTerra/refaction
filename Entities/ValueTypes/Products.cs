using refactor_me.DomainObjects.Entities;
using System.Collections.Generic;
using System.Linq;

namespace refactor_me.DomainObjects.ValueTypes
{
    /// <summary>
    /// Represents a list of <see cref="Product"/>s
    /// </summary>
    public class Products
    {
        /// <summary>
        /// The list of items
        /// </summary>
        /// <remarks>
        /// This is just to make sure the JSON renders as expected
        /// </remarks>
        public IEnumerable<Product> Items { get; }

        /// <summary>
        /// A list of all the <see cref="Product"/>s
        /// </summary>
        public Products(IEnumerable<Product> items)
        {
            this.Items = items;
        }

        public override string ToString()
        {
            return $"{nameof(Products)}[{Items?.Count()}]";
        }
    }
}