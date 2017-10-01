using refactor_me.Dal.Sql;
using refactor_me.Dal.Sql.Repositories;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.ValueTypes;
using refactor_me.Services;
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
            try
            {
                return _service.GetAllProducts();
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            try
            {
                return _service.SearchByName(name);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            try
            {
                return _service.GetProduct(id);
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            try
            {
                _service.CreateProduct(product);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            try
            {
                _service.UpdateProduct(product);
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            try
            {
                _service.DeleteProduct(id);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            try
            {
                return _service.GetProductOptions(productId);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid optionId)
        {
            try
            {
                return _service.GetProductOption(productId, optionId);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
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

            try
            {
                _service.UpdateProductOption(orig);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            try
            {
                _service.DeleteProductOption(id);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
