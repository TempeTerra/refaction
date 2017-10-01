using System;
using refactor_me.Entities;
using refactor_me.Services.Models;

namespace refactor_me.Services
{
    public interface IProductsService
    {
        /// <summary>
        /// Add the supplied Product to permanent storage
        /// </summary>
        /// <param name="product">A fully specified new Product</param>
        void CreateProduct(Product product);

        /// <summary>
        /// Add a new ProductOption to permanent storage
        /// </summary>
        /// <param name="option">A fully specified new ProductOption</param>
        void CreateProductOption(ProductOption option);

        /// <summary>
        /// Delete an existing Product and any associated ProductOptions
        /// </summary>
        /// <param name="productId">The ID of the Product</param>
        void DeleteProduct(Guid productId);

        /// <summary>
        /// Delete an existing ProductOption
        /// </summary>
        /// <param name="id">The ProductOption ID</param>
        void DeleteProductOption(Guid id);

        /// <summary>
        /// Get all Products currently in permanent storage
        /// </summary>
        /// <returns>A <see cref="Products"/> instance where Items contains the requested <see cref="Product"/>s</returns>
        Products GetAllProducts();

        /// <summary>
        /// Get an existing Product by ID
        /// </summary>
        /// <param name="id">The Product ID</param>
        /// <returns>A Product</returns>
        Product GetProduct(Guid id);

        /// <summary>
        /// Get an existing ProductOption for an existing Product
        /// The ProductOption must belong to the Product
        /// </summary>
        /// <param name="productId">The Product ID</param>
        /// <param name="optionId">The ProductOption ID</param>
        /// <returns>A ProductOption</returns>
        ProductOption GetProductOption(Guid productId, Guid optionId);

        /// <summary>
        /// Gets all the ProductOptions for an existing Product
        /// </summary>
        /// <param name="productId">The Product ID</param>
        /// <returns>A <see cref="ProductOptions"/> instance where Items contains the requested <see cref="ProductOption"/>s</returns>
        ProductOptions GetProductOptions(Guid productId);

        /// <summary>
        /// Finds all Products where the Name property is like the supplied pattern
        /// </summary>
        /// <param name="namePattern">A string to be used in a case-insensitive substring match against Name properties</param>
        /// <returns>A <see cref="Products"/> instance where Items contains the matching <see cref="Product"/>s</returns>
        Products SearchByName(string namePattern);

        /// <summary>
        /// Update an existing Product to match the supplied object
        /// </summary>
        /// <param name="product">A fully specified Product which will replace the existing stored data with the same ID</param>
        void UpdateProduct(Product product);

        /// <summary>
        /// Update an existing ProductOption to match the supplied object
        /// </summary>
        /// <param name="product">A fully specified ProductOption which will replace the existing stored data with the same ID</param>
        void UpdateProductOption(ProductOption orig);
    }
}