using Newtonsoft.Json;
using refactor_me.Models.Base;
using System;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    /// <summary>
    /// A high-level description of a product.
    /// 
    /// May have <see cref="ProductOption"/>s to specify different subtypes, 
    /// such as different colours.
    /// </summary>
    public class Product : BaseModel
    {
        /// <summary>
        /// The name of the Product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the Product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the Product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The additional price to have the product delivered (?)
        /// </summary>
        public decimal DeliveryPrice { get; set; }

        /// <summary>
        /// Create a new Product
        /// </summary>
        /// <remarks>
        /// This will also get called by the model binder when data is
        /// sent to the controller over HTTP
        /// </remarks>
        public Product()
            : base(isNew: true)
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Retrieve an existing Product from storage
        /// </summary>
        /// <param name="id">The id of an existing stored product</param>
        public Product(Guid id)
            : base(isNew: false)
        {
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select * from product where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                var rdr = cmd.ExecuteReader();

                // Exception if no matching record is found in the database
                // -- An alternative would be to return a new Product instead,
                // -- perhaps with the supplied id, but that goes against the 
                // -- intended behaviour of this class and is probably not what
                // -- the caller expects.
                if (!rdr.Read())
                {
                    throw new ArgumentException($"No {nameof(Product)} was found with Id {id}");
                }

                Id = Guid.Parse(rdr["Id"].ToString());
                Name = rdr["Name"].ToString();
                Description = (DBNull.Value == rdr["Description"])
                    ? null
                    : rdr["Description"].ToString();
                Price = decimal.Parse(rdr["Price"].ToString());
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
            }
        }

        /// <summary>
        /// Save the current state of the Product
        /// </summary>
        public void Save()
        {
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                SqlCommand cmd;
                if (IsNew)
                {
                    cmd = new SqlCommand("insert into product (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)", conn);
                }
                else
                {
                    cmd = new SqlCommand("update product set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id", conn);
                }

                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@DeliveryPrice", DeliveryPrice);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete the Product and also any associated <see cref="ProductOption"/>s
        /// </summary>
        public void Delete()
        {
            // Delete related ProductOptions
            foreach (var option in new ProductOptions(Id).Items)
            {
                option.Delete();
            }

            // Delete the record for this instance
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand($"delete from product where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}