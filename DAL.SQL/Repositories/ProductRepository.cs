using refactor_me.Dal.Repositories;
using refactor_me.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace refactor_me.Dal.Sql.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public ProductRepository(ConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public Product[] GetAll()
        {
            var Items = new List<Product>();
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select id from product", conn);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    Items.Add(Get(id));
                }
            }

            return Items.ToArray();
        }

        public Product[] SearchByName(string pattern)
        {
            var Items = new List<Product>();
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select id from product where name COLLATE Latin1_General_CI_AS like @Pattern", conn);
                cmd.Parameters.AddWithValue("@Pattern", $"%{pattern.ToLower()}%");

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    Items.Add(Get(id));
                }
            }

            return Items.ToArray();
        }

        public Product Get(Guid id)
        {
            Product entity = new Product(id);

            using (var conn = _connectionFactory.NewConnection())
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

                entity.Id = Guid.Parse(rdr["Id"].ToString());
                entity.Name = rdr["Name"].ToString();
                entity.Description = (DBNull.Value == rdr["Description"])
                    ? null
                    : rdr["Description"].ToString();
                entity.Price = decimal.Parse(rdr["Price"].ToString());
                entity.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
            }

            return entity;
        }

        public void Create(Product entity)
        {
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("insert into product (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);
                cmd.Parameters.AddWithValue("@Price", entity.Price);
                cmd.Parameters.AddWithValue("@DeliveryPrice", entity.DeliveryPrice);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Product entity)
        {
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("update product set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);
                cmd.Parameters.AddWithValue("@Price", entity.Price);
                cmd.Parameters.AddWithValue("@DeliveryPrice", entity.DeliveryPrice);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand($"delete from product where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
