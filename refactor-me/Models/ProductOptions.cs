﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions();
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions(productId);
        }

        /// <summary>
        /// Load all ProductOptions
        /// </summary>
        private void LoadProductOptions()
        {
            Items = new List<ProductOption>();
            using (var conn = Helpers.NewConnection())
            {
                var cmd = new SqlCommand("select id from productoption", conn);
                conn.Open();

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    Items.Add(new ProductOption(id));
                }
            }
        }

        /// <summary>
        /// Load ProductOptions for a specific <see cref="Product"/>
        /// </summary>
        /// <param name="productId">The id of the owning Product</param>
        private void LoadProductOptions(Guid productId)
        {
            Items = new List<ProductOption>();
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select id from productoption where productid = @ProductId", conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    Items.Add(new ProductOption(id));
                }
            }
        }
    }
}