using refactor_me.Dal.Sql;
using refactor_me.Dal.Sql.Repositories;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.ValueTypes;
using refactor_me.Services;
using System;
using System.Configuration;
using System.Web.Http;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductsService _service;

        public ProductsController()
        {
            // -- This is all ripe for dependency injection
            string entityStorageConnectionString = ConfigurationManager.ConnectionStrings["EntityStorage"].ConnectionString;
            var connectionFactory = new ConnectionFactory(entityStorageConnectionString);
            var productOptionRepository = new ProductOptionRepository(connectionFactory);
            var productRepository = new ProductRepository(connectionFactory);

            this._service = new ProductsService(productRepository, productOptionRepository);
        }

        [Route]
        [HttpGet]
        public Products GetAll()
        {
            return _service.GetAllProducts();
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            return _service.SearchByName(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            return _service.GetProduct(id);
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _service.CreateProduct(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product orig)
        {
            // Mapping values onto a new Product here avoids an
            // auto-generated ID and sets the IsNew flag properly
            var product = new Product(id)
            {
                Name = orig.Name,
                Description = orig.Description,
                Price = orig.Price,
                DeliveryPrice = orig.DeliveryPrice
            };

            _service.UpdateProduct(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _service.DeleteProduct(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return _service.GetProductOptions(productId);
        }

        [Route("{productId}/options/{optionId}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid optionId)
        {
            return _service.GetProductOption(productId, optionId);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;

            _service.CreateProductOption(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            // Mapping values onto a new Product here avoids an
            // auto-generated ID and sets the IsNew flag properly
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            _service.UpdateProductOption(orig);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            _service.DeleteProductOption(id);
        }
    }
}
