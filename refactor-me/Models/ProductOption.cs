using Newtonsoft.Json;
using refactor_me.Models.Base;
using System;
using System.Data.SqlClient;

namespace refactor_me.Models
{
    public class ProductOption : BaseModel
    {
        /// <summary>
        /// The ID of the product this option applies to
        /// </summary>
        [JsonIgnore]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Create a new ProductOption
        /// </summary>
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
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();

            // Exception if no matching record is found in the database
            if (!rdr.Read())
            {
                throw new ArgumentException($"No {nameof(Product)} was found with id {id}");
            }

            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew ?
                new SqlCommand($"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')", conn) :
                new SqlCommand($"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from productoption where id = '{Id}'", conn);
            cmd.ExecuteReader();
        }
    }
}