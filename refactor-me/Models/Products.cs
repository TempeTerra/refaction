using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts();
        }

        public Products(string pattern)
        {
            LoadProducts(pattern);
        }

        /// <summary>
        /// Load all products
        /// </summary>
        private void LoadProducts()
        {
            Items = new List<Product>();
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand($"select id from product", conn);
                
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    Items.Add(new Product(id));
                }
            }
        }

        /// <summary>
        /// Load Products with name matching a pattern
        /// </summary>
        /// <param name="pattern">A pattern to be used with SQL LIKE on the product name</param>
        /// <remarks>
        /// The pattern is matched as a substring of the name colum, case insensitive
        /// </remarks>
        private void LoadProducts(string pattern)
        {
            Items = new List<Product>();
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand($"select id from product where name COLLATE UTF8_GENERAL_CI like %@Pattern%", conn);
                cmd.Parameters.AddWithValue("@Pattern", pattern);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    Items.Add(new Product(id));
                }
            }
        }
    }
}