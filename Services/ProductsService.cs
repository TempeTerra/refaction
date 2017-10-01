using refactor_me.Dal.Repositories;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.ValueTypes;
using refactor_me.Services.Exceptions;
using System;
using System.Data.SqlClient;

namespace refactor_me.Services
{
    /// <summary>
    /// Provides CRUD operations for Product and ProductOption entities
    /// </summary>
    /// <remarks>
    /// Storage operations are passed off to repositories.
    /// -- Exceptions from the storage layer are wrapped in a custom exception
    /// -- if they are recognised as something which is an expected failure mode.
    /// -- Unexpected exceptions are rethrown unmodified. This allows callers to
    /// -- filter out normal failure operations (custom exceptions) but still
    /// -- appropriately escalate unexpected situations.
    /// </remarks>
    public class ProductsService : IProductsService
    {
        // -- injected dependencies; data access layer repositories
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly IProductRepository _productRepository;

        public ProductsService(
            IProductRepository productRepository,
            IProductOptionRepository productOptionRepository)
        {
            this._productRepository = productRepository;
            this._productOptionRepository = productOptionRepository;
        }

        public Products GetAllProducts()
        {
            try
            {
                return new Products(_productRepository.GetAll());
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Exception while retrieving all Products from storage", e);
            }
        }

        public Products SearchByName(string pattern)
        {
            try
            {
                return new Products(_productRepository.SearchByName(pattern));
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Exception while searching for Products with Name like '{pattern}' in storage", e);
            }
        }

        public Product GetProduct(Guid id)
        {
            try
            {
                return _productRepository.Get(id);
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Couldn't find Product with ID {id} in storage", e);
            }
        }

        public void CreateProduct(Product product)
        {
            if(product.IsNew == false)
            {
                // -- I wouldn't usually use exceptions for 'signalling', but
                // -- in this case trying to recreate an existing product is
                // -- a misuse of the service and the caller should know
                // -- better. If the callers want an Exists(productId) method
                // -- we should support that explicitly.
                throw new NotANewEntityException($"Product {product} can't be created because it is not new (IsNew property)", product);
            }

            try
            {
                _productRepository.Create(product);
            }
            catch (SqlException e)
            {
                //-- Wrap the db exception with a slightly more meaningful message
                throw new OperationFailedException($"Failed to create product {product} in storage", e);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                _productRepository.Update(product);
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Failed to update product {product} in storage", e);
            }
        }

        public void DeleteProduct(Guid productId)
        {
            // -- TODO transaction
            // -- These deletes should happen inside a transaction so the database can't
            // -- end up in an inconsistent state. I'd tend to do it with a unit of work,
            // -- provided as a parameter to the repositories with the delete calls.

            // Delete related ProductOptions
            var relatedProductOptions = _productOptionRepository.GetForProduct(productId);
            foreach (var option in relatedProductOptions)
            {
                _productOptionRepository.Delete(option.Id);
            }

            // Delete the Product
            _productRepository.Delete(productId);
        }

        public ProductOptions GetProductOptions(Guid productId)
        {
            try
            {
                return new ProductOptions(_productOptionRepository.GetForProduct(productId));
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Failed to get ProductOptions for Product with ID {productId}", e);
            }
        }

        public ProductOption GetProductOption(Guid productId, Guid optionId)
        {
            ProductOption option;

            try
            {
                option = _productOptionRepository.Get(optionId);
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Failed to get ProductOption with ID {optionId}", e);
            }

            // check that the option actually applies to the expected product
            if (option.ProductId != productId)
            {
                throw new ProductOptionMismatchException($"Invalid option request: option {optionId} \"{option.Name}\" is not available for product {productId}.");
            }

            return option;
        }

        public void CreateProductOption(ProductOption option)
        {
            if (option.IsNew == false)
            {
                throw new NotANewEntityException($"ProductOption {option} can't be created because it is not new (IsNew property)", option);
            }

            try
            {
                _productOptionRepository.Create(option);
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Failed to create ProductOption {option} in database", e);
            }
        }

        public void UpdateProductOption(ProductOption option)
        {
            try
            {
                _productOptionRepository.Update(option);
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Failed to update ProductOption {option} in database", e);
            }
        }

        public void DeleteProductOption(Guid id)
        {
            try
            {
                _productOptionRepository.Delete(id);
            }
            catch (SqlException e)
            {
                throw new OperationFailedException($"Failed to delete ProductOption with ID {id} in database", e);
            }
        }
    }
}
