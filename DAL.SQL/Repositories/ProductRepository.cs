using refactor_me.Dal.Repositories;
using refactor_me.DomainObjects.Entities;
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
                    Items.Add(FromReader(rdr));
                }
            }

            return Items.ToArray();
        }

        public Product[] SearchByName(string pattern)
        {
            if (pattern == null)
            {
                // This could also be handled by treating null as empty string,
                // or by returning no results. I'm inclined to throw a description
                // exception until further behavior specification is available.
                throw new ArgumentNullException(nameof(pattern), "SearchByName pattern must not be null");
            }

            var Items = new List<Product>();
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select id from product where name COLLATE Latin1_General_CI_AS like @Pattern", conn);
                cmd.Parameters.AddWithValue("@Pattern", $"%{pattern.ToLower()}%");

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Items.Add(FromReader(rdr));
                }
            }

            return Items.ToArray();
        }

        public Product Get(Guid id)
        {
            Product entity;

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

                entity = FromReader(rdr);
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

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    throw new NoRowsCreatedException(entity);
                }
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

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new NoRowsUpdatedException(entity);
                }
                else if (rowsAffected > 1)
                {
                    throw new DuplicateIdException(entity);
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand($"delete from product where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new DeleteIdNotFoundException("Product", id);
                }
                else if (rowsAffected > 1)
                {
                    throw new DuplicateIdException("Product", id);
                }
            }
        }

        private Product FromReader(SqlDataReader rdr)
        {
            Guid id = Guid.Parse(rdr["Id"].ToString());

            Product result = new Product(id)
            {
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"])
                    ? null
                    : rdr["Description"].ToString(),
                Price = decimal.Parse(rdr["Price"].ToString()),
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString()),
            };

            return result;
        }
    }
}
