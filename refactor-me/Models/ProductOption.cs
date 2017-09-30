using Newtonsoft.Json;
using refactor_me.Models.Base;
using System;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    /// <summary>
    /// An optional subtype of a <see cref="Product"/>, for example a colour variant
    /// </summary>
    public class ProductOption : BaseModel
    {
        /// <summary>
        /// The ID of the product this option applies to
        /// </summary>
        [JsonIgnore]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The full product name including the option variant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Create a new ProductOption
        /// </summary>
        /// <remarks>
        /// This will also get called by the model binder when data is
        /// sent to the controller over HTTP
        /// </remarks>
        public ProductOption()
            : base(isNew: true)
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Retrieve an existing ProductOption from storage
        /// </summary>
        /// <param name="id">The id of an existing stored ProductOption</param>
        public ProductOption(Guid id)
            : base(isNew: false)
        {
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("select * from productoption where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                var rdr = cmd.ExecuteReader();

                // Exception if no matching record is found in the database
                if (!rdr.Read())
                {
                    throw new ArgumentException($"No {nameof(Product)} was found with id {id}");
                }

                Id = Guid.Parse(rdr["Id"].ToString());
                ProductId = Guid.Parse(rdr["ProductId"].ToString());
                Name = rdr["Name"].ToString();
                Description = (DBNull.Value == rdr["Description"]) 
                    ? null 
                    : rdr["Description"].ToString();
            }
        }

        /// <summary>
        /// Save the current state of this ProductOption
        /// </summary>
        public void Save()
        {
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                SqlCommand cmd;
                if (IsNew)
                {
                    cmd = new SqlCommand($"insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)", conn);
                }
                else
                {
                    cmd = new SqlCommand($"update productoption set name = @Name, description = @Description where id = @Id", conn);
                }

                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@ProductId", ProductId); // this is not used in the second query, test to check if it's a problem
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Description", Description);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete the record for this ProductOption from permanent storage
        /// </summary>
        public void Delete()
        {
            using (var conn = Helpers.NewConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("delete from productoption where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.ExecuteReader();
            }
        }
    }
}