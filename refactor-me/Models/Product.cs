using Newtonsoft.Json;
using refactor_me.Models.Base;
using System;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        /// <summary>
        /// Create a new Product
        /// </summary>
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
                var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
                conn.Open();

                var rdr = cmd.ExecuteReader();

                // Exception if no matching record is found in the database
                // -- An alternative would be to return a new Product instead,
                // -- perhaps with the supplied id, but that goes against the 
                // -- intended behaviour of this class and is probably not what
                // -- the caller intended.
                if (!rdr.Read())
                {
                    throw new ArgumentException($"No {nameof(Product)} was found with id {id}");
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

        public void Save()
        {
            using (var conn = Helpers.NewConnection())
            {
                var cmd = IsNew
                    ? new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})", conn)
                    : new SqlCommand($"update product set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}'", conn);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete this Product and also any associated <see cref="ProductOption"/>s
        /// </summary>
        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
            {
                option.Delete();
            }

            using (var conn = Helpers.NewConnection())
            {
                conn.Open();
                var cmd = new SqlCommand($"delete from product where id = '{Id}'", conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}