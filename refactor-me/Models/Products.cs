using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    /// <summary>
    /// Represents a list of <see cref="Product"/>s
    /// </summary>
    public class Products
    {
        /// <summary>
        /// The list of items
        /// </summary>
        /// <remarks>
        /// This is just to make sure the JSON renders as expected
        /// </remarks>
        public List<Product> Items { get; private set; }

        /// <summary>
        /// A list of all the <see cref="Product"/>s
        /// </summary>
        public Products()
        {
            LoadProducts();
        }

        /// <summary>
        /// The list of <see cref="Product"/>s with names matching the supplied pattern
        /// </summary>
        /// <param name="pattern">A pattern for a case-insensitive substring search of Product names</param>
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
            Items.Clear();
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand($"select id from product where name COLLATE Latin1_General_CI_AS like @Pattern", conn);
                cmd.Parameters.AddWithValue("@Pattern", $"%{pattern.ToLower()}%");

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