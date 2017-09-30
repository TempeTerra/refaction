using System;
using refactor_me.Entities;
using refactor_me.Services.Models;

namespace refactor_me.Services
{
    public interface IProductsService
    {
        void CreateProduct(Product product);
        void CreateProductOption(ProductOption option);
        void DeleteProduct(Guid productId);
        void DeleteProductOption(Guid id);
        Products GetAllProducts();
        Product GetProduct(Guid id);
        ProductOption GetProductOption(Guid productId, Guid optionId);
        ProductOptions GetProductOptions(Guid productId);
        Products SearchByName(string name);
        void Update(Product product);
        void UpdateProductOption(ProductOption orig);
    }
}