using refactor_me.Dal.Repositories;
using refactor_me.DomainObjects.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace refactor_me.Dal.Sql.Repositories
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public ProductOptionRepository(ConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public ProductOption[] GetAll()
        {
            var Items = new List<ProductOption>();
            using (var conn = _connectionFactory.NewConnection())
            {
                var cmd = new SqlCommand("select id from productoption", conn);
                conn.Open();

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Items.Add(FromReader(rdr));
                }
            }

            return Items.ToArray();
        }

        public ProductOption[] GetForProduct(Guid productId)
        {
            var Items = new List<ProductOption>();
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select id from productoption where productid = @ProductId", conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Items.Add(FromReader(rdr));
                }
            }

            return Items.ToArray();
        }

        public ProductOption Get(Guid id)
        {
            var result = new ProductOption(id);

            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select * from productoption where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                var rdr = cmd.ExecuteReader();

                // Exception if no matching record is found in the database
                if (!rdr.Read())
                {
                    throw new ArgumentException($"No {nameof(Product)} was found with id {id}");
                }

                result = FromReader(rdr);
            }

            return result;
        }

        public void Create(ProductOption entity)
        {
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@ProductId", entity.ProductId); // this is not used in the second query, test to check if it's a problem
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    throw new NoRowsCreatedException(entity);
                }
            }
        }

        public void Update(ProductOption entity)
        {
            using (var conn = _connectionFactory.NewConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"update productoption set name = @Name, description = @Description where id = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);

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

                var cmd = new SqlCommand("delete from productoption where id = @Id", conn);
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

        private ProductOption FromReader(SqlDataReader rdr)
        {
            // Get the ID first so we can construct the entity with IsNew = false
            Guid id = Guid.Parse(rdr["Id"].ToString());

            var result = new ProductOption(id)
            {
                ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"])
                    ? null
                    : rdr["Description"].ToString()
            };

            return result;
        }
    }
}
