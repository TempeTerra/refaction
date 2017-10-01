using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Dal.Sql;
using refactor_me.Dal.Sql.Repositories;
using refactor_me.DomainObjects.Entities;
using refactor_me.DomainObjects.ValueTypes;
using refactor_me.Services;
using refactor_me.Services.Exceptions;
using System;
using System.IO;
using System.Linq;
using Tests.TestData.StaticEntities.ProductOptions;
using Tests.TestData.StaticEntities.Products;

namespace Tests
{
    [DeploymentItem(@"TestData\Database.mdf")]
    [DeploymentItem(@"TestData\Database_log.ldf")]
    [TestClass]
    public class TestProductsService
    {
        private ProductsService _service;

        [TestInitialize]
        public void Init()
        {
            // Create Service and dependencies

            // -- Boy I was confused until I figured out that a relative path doesn't work for some reason
            string testDatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Database.mdf");
            string entityStorageConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + testDatabasePath + @";Integrated Security=True";
            var connectionFactory = new ConnectionFactory(entityStorageConnectionString);
            var productOptionRepository = new ProductOptionRepository(connectionFactory);
            var productRepository = new ProductRepository(connectionFactory);

            this._service = new ProductsService(productRepository, productOptionRepository);

            // Scrub or reset test database before each test
            using (var conn = connectionFactory.NewConnection())
            {
                conn.Open();

                var truncateProductsCommand = conn.CreateCommand();
                truncateProductsCommand.CommandText = "TRUNCATE TABLE Product";
                truncateProductsCommand.ExecuteNonQuery();

                var truncateProductOptionsCommand = conn.CreateCommand();
                truncateProductOptionsCommand.CommandText = "TRUNCATE TABLE ProductOption";
                truncateProductOptionsCommand.ExecuteNonQuery();
            }

            // Insert default data
            // -- this assumes that the service basically works even before the
            // -- tests are run, but it's very convenient. Could bulk insert SQL
            // -- or something instead.
            _service.CreateProduct(Iphone6s.Instance);
            _service.CreateProduct(GalaxyS7.Instance);

            _service.CreateProductOption(Iphone6sWhite.Instance);
            _service.CreateProductOption(Iphone6sBlack.Instance);
            _service.CreateProductOption(Iphone6sRoseGold.Instance);
            _service.CreateProductOption(GalaxyS7White.Instance);
            _service.CreateProductOption(GalaxyS7Black.Instance);

        }

        /// <summary>
        /// Check that the prepared test data matches our expectations
        /// </summary>
        [TestMethod]
        public void CheckTestData()
        {
            var products = _service.GetAllProducts();
            Assert.AreEqual(2, products.Items.Count());

            var iPhoneOptions = _service.GetProductOptions(Iphone6s.Instance.Id);
            Assert.AreEqual(3, iPhoneOptions.Items.Count());

            var galaxyOptions = _service.GetProductOptions(GalaxyS7.Instance.Id);
            Assert.AreEqual(2, galaxyOptions.Items.Count());
        }

        /// <summary>
        /// Test that the product service has been instantiated
        /// </summary>
        [TestMethod]
        public void CreateProductsService()
        {
            //Trivial check that the service was created in the Init method
            Assert.IsNotNull(_service);
        }

        /// <summary>
        /// Test GetAllProducts, including some checks that the results look valid
        /// </summary>
        [TestMethod]
        public void GetAllProducts()
        {
            var result = _service.GetAllProducts();

            // -- The database should contain the default data; 2 products
            Assert.AreEqual(2, result.Items.Count());

            // check some basic correctness
            Assert.IsFalse(result.Items.Any(x => x == null));
            Assert.IsFalse(result.Items.Any(x => x.Id == null));
            Assert.IsFalse(result.Items.Any(x => x.Id == default(Guid)));
        }

        /// <summary>
        /// Test getting products using the name pattern matching function
        /// </summary>
        [TestMethod]
        public void GetNamedProducts()
        {
            Products result;

            // check something useful happens for malformed input
            try
            {
                result = _service.SearchByName(null);
                Assert.Fail("Passing a NULL search pattern should cause an exception");
            }
            catch (ArgumentNullException)
            {
                // Good, carry on.
            }

            // Searching to match an empty string trivially matches everything
            // Behaviour should be clarified here, but returning all results
            // isn't completely stupid in the interim.
            result = _service.SearchByName(string.Empty);
            Assert.AreEqual(2, result.Items.Count());

            // check a miss returns an empty list
            result = _service.SearchByName("nothing");
            Assert.IsFalse(result.Items.Any());

            // check an exact match on Name
            result = _service.SearchByName("Apple iPhone 6S");
            Assert.AreEqual(1, result.Items.Count());

            // check case insensitive match works
            result = _service.SearchByName("applE IpHone 6s");
            Assert.AreEqual(1, result.Items.Count());

            // check startswith match works
            result = _service.SearchByName("applE");
            Assert.AreEqual(1, result.Items.Count());

            // check endswith match works
            result = _service.SearchByName("6s");
            Assert.AreEqual(1, result.Items.Count());

            // check contains match works
            result = _service.SearchByName("IpHone");
            Assert.AreEqual(1, result.Items.Count());

            // check multiple results for multiple matches
            result = _service.SearchByName("S");
            Assert.AreEqual(2, result.Items.Count());
        }

        /// <summary>
        /// Test getting a Product by supplying its ID
        /// </summary>
        [TestMethod]
        public void GetProductById()
        {
            // Look up iPhone by ID
            var result = _service.GetProduct(Iphone6s.Instance.Id);
            Assert.AreEqual(Iphone6s.Instance.Name, result.Name);

            // look up Galaxy by ID
            result = _service.GetProduct(GalaxyS7.Instance.Id);
            Assert.AreEqual(GalaxyS7.Instance.Name, result.Name);
        }

        /// <summary>
        /// Test creating a new Product
        /// </summary>
        [TestMethod]
        public void CreateProduct()
        {
            Product newProduct = new Product()
            {
                // Id is automatically generated
                Name = "ShinyPhone Double-S",
                Description = "The phone with more S's than the competition!",
                Price = 1499.95M, // Those S's don't come cheap
                DeliveryPrice = 10.95M
            };

            // we're testing the expected case where the product really is new
            Assert.IsTrue(newProduct.IsNew);

            _service.CreateProduct(newProduct);

            // Check retrieval, lookup by the auto-generated ID
            // Should be different ID on each run
            var result = _service.GetProduct(newProduct.Id);

            // Checking the ID we just queried by seems excessive, but
            // it might show up a problem in the deserialization of the result
            Assert.AreEqual(newProduct.Id, result.Id);
            Assert.AreEqual(newProduct.Name, result.Name);
        }

        /// <summary>
        /// Make sure trying to re-create an existing product (based on IsNew flag) fails
        /// </summary>
        [TestMethod]
        public void CreateProduct_ExistingProduct()
        {
            // Check that trying to recreate an existing product fails
            var existingProduct = _service.GetProduct(Iphone6s.Instance.Id);
            // replace the ID with a new one (this is a bad idea, we should
            // probably make it impossible)
            existingProduct.Id = Guid.NewGuid();

            // the product should be flagged as not new
            Assert.IsFalse(existingProduct.IsNew);

            try
            {
                _service.CreateProduct(existingProduct);
                Assert.Fail("Successfully passed an existing product as a new product");
            }
            catch (NotANewEntityException)
            {
                // Good, trying to create a product which already exists, even if it's flagged
                // as new, shouldn't work
            }
        }

        /// <summary>
        /// Make sure trying to create a product with an existing ID fails
        /// </summary>
        [TestMethod]
        public void CreateProduct_DuplicateId()
        {
            // Also try a nominally new product with an existing ID
            // This is created through the new product constructor which
            // internally generates a new ID - but we can still change it
            // afterwards. The product will claim to be new.
            Product newProduct = new Product()
            {
                Id = Iphone6s.Instance.Id,
                Name = "Pear pHone 6S",
                Description = "It looks a lot like an iPhone to be honest",
                Price = Iphone6s.Instance.Price - 50M,
                DeliveryPrice = Iphone6s.Instance.DeliveryPrice
            };

            // Check that the product claims to be new
            Assert.IsTrue(newProduct.IsNew);

            try
            {
                _service.CreateProduct(newProduct);
                Assert.Fail("Successfully created a product with duplicate ID");
            }
            catch (OperationFailedException)
            {
                // Good, supplying a duplicate ID should raise an ArgumentException
            }
        }

        /// <summary>
        /// Check that products can't be created with an invalid ID
        /// </summary>
        /// <remarks>
        /// This is actually caught in the Product constructor rather than
        /// the repository layer
        /// </remarks>
        [TestMethod]
        public void CreateProduct_BadId()
        {
            try
            {
                Product newProduct = new Product()
                {
                    Id = Guid.Empty,
                    Name = "NullPhone Null",
                    Description = "How much more null could it be? Null. Null more null.",
                    Price = 0M,
                    DeliveryPrice = 0M
                };

                Assert.Fail("Created a product with default/empty ID");
            }
            catch (ArgumentException)
            {
                // Good, supplying a default ID should raise an Argument Exception
            }
        }

        /// <summary>
        /// Test updating an existing product
        /// </summary>
        [TestMethod]
        public void UpdateProduct()
        {
            var product = Iphone6s.Instance;

            // change descripton
            // -- don't really want to change the static instance (it should
            // -- be immutable ideally)
            product.Description = "6 is a perfectly good version number, not like that vulgar 7";

            _service.UpdateProduct(product);

            var result = _service.GetProduct(Iphone6s.Instance.Id);

            Assert.AreEqual(product.Description, result.Description);
        }

        /// <summary>
        /// Test deleting a Product (and its related ProductOptions)
        /// </summary>
        [TestMethod]
        public void DeleteProduct()
        {
            _service.DeleteProduct(GalaxyS7.Instance.Id);

            // this should now fail
            try
            {
                var result = _service.GetProduct(GalaxyS7.Instance.Id);
                Assert.Fail("No exception thrown when requesting a deleted product");
            }
            catch (ArgumentException)
            {
                // Good, we should get an exception warning about invalid ID
            }

            // Even though the parent ID is missing, the result should
            // be an empty list of options
            var options = _service.GetProductOptions(GalaxyS7.Instance.Id);
            Assert.IsFalse(options.Items.Any());
        }

        [TestMethod]
        public void GetOptionsForProduct()
        {
            var options = _service.GetProductOptions(Iphone6s.Instance.Id);

            Assert.AreEqual(3, options.Items.Count());
        }

        [TestMethod]
        public void GetOptionById()
        {
            //TODO note double ID required
            var option = _service.GetProductOption(Iphone6s.Instance.Id, Iphone6sRoseGold.Instance.Id);
        }

        [TestMethod]
        public void CreateProductOption()
        {
            var newOption = new ProductOption()
            {
                // Id is generated automatically
                ProductId = Iphone6s.Instance.Id,
                Name = "Ocatrine",
                Description = "Octarine Apple iPhone 6S"
            };

            Assert.IsTrue(newOption.IsNew);

            _service.CreateProductOption(newOption);

            // check that we can get the new option by ID
            var result = _service.GetProductOption(Iphone6s.Instance.Id, newOption.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(newOption.Name, result.Name);

            // also check that the product option count has increased
            var options = _service.GetProductOptions(Iphone6s.Instance.Id);
            Assert.AreEqual(4, options.Items.Count());
        }

        [TestMethod]
        public void UpdateProductOption()
        {
            var option = Iphone6sRoseGold.Instance;

            // change descripton
            // -- don't really want to change the static instance (it should
            // -- be immutable ideally)
            option.Description = "Rose Gold Apple iPhone 6S";

            _service.UpdateProductOption(option);

            var result = _service.GetProductOption(Iphone6s.Instance.Id, Iphone6sRoseGold.Instance.Id);

            Assert.AreEqual(option.Description, result.Description);
        }

        [TestMethod]
        public void DeleteProductOption()
        {
            _service.DeleteProductOption(GalaxyS7White.Instance.Id);

            // this should now fail
            try
            {
                var result = _service.GetProductOption(GalaxyS7.Instance.Id, GalaxyS7White.Instance.Id);
                Assert.Fail("No exception thrown when requesting a deleted product");
            }
            catch (ArgumentException)
            {
                // Good, we should get an exception warning about invalid ID
            }

            // Product should now have only 1 option remaining
            var options = _service.GetProductOptions(GalaxyS7.Instance.Id);
            Assert.AreEqual(1, options.Items.Count());
        }
    }
}
