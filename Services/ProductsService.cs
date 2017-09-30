using refactor_me.Dal.Repositories;
using refactor_me.Entities;
using refactor_me.Services.Models;
using System;

namespace refactor_me.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly IProductRepository _productRepository;

        public ProductsService(
            IProductRepository productRepository, 
            IProductOptionRepository productOptionRepository)
        {
            this._productRepository = productRepository;
            this._productOptionRepository = productOptionRepository;
        }

        public void DeleteProduct(Guid productId)
        {
            // Delete related ProductOptions
            var relatedProductOptions = _productOptionRepository.GetForProduct(productId);
            foreach (var option in relatedProductOptions)
            {
                _productOptionRepository.Delete(option.Id);
            }

            // Delete the Product
            _productRepository.Delete(productId);
        }

        public Products GetAllProducts()
        {
            return new Products(_productRepository.GetAll());
        }

        public Products SearchByName(string name)
        {
            return new Products(_productRepository.SearchByName(name));
        }

        public Product GetProduct(Guid id)
        {
            return _productRepository.Get(id);
        }

        public void CreateProduct(Product product)
        {
            _productRepository.Create(product);
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }

        public ProductOptions GetProductOptions(Guid productId)
        {
            return new ProductOptions(_productOptionRepository.GetForProduct(productId));
        }

        public ProductOption GetProductOption(Guid productId, Guid optionId)
        {
            var option = _productOptionRepository.Get(optionId);

            if(option.ProductId != productId)
            {
                //TODO custom exception
                throw new Exception($"Invalid option request: option {optionId} \"{option.Name}\" is not available for product {productId}.");
            }

            return option;
        }

        public void CreateProductOption(ProductOption option)
        {
            _productOptionRepository.Create(option);
        }

        public void UpdateProductOption(ProductOption option)
        {
            _productOptionRepository.Update(option);
        }

        public void DeleteProductOption(Guid id)
        {
            _productOptionRepository.Delete(id);
        }
    }
}
