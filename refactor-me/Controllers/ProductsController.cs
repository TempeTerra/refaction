using refactor_me.Dal.Sql;
using refactor_me.Dal.Sql.Repositories;
using refactor_me.Entities;
using refactor_me.Services;
using refactor_me.Services.Models;
using System;
using System.Configuration;
using System.Net;
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
            try
            {
                return _service.GetProduct(id);
            }
            catch(Exception e)
            {
                // TODO selective error handling, or meaningful result from service
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _service.CreateProduct(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            // TODO separate ID is awkward
            _service.Update(product);

            //TODO remember to check for existing product
            //if (!orig.IsNew)
            //    orig.Save();
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

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid optionId)
        {
            //TODO product id not actually used
            // better check it to avoid showing an invalid option for a 
            // product
            //if (option.IsNew)
            //    throw new HttpResponseException(HttpStatusCode.NotFound);

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
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            //TODO
            //if (!orig.IsNew)
            //    orig.Save();

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
