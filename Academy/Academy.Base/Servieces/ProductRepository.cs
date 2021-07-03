using Academy.Base.Model;
 using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Base.Servieces
{
    public class ProductRepository : IProductRepository
    {
        private readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Academy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public int Create(Product product)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Product(Name,Price,Quantity)  
                              VALUES (@name, @price, @quantity)
                              SELECT @id = SCOPE_IDENTITY()"; 

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@name", product.Name));
                command.Parameters.Add(new SqlParameter("@price", product.Price));
                command.Parameters.Add(new SqlParameter("@quantity", product.Quantity)); 

                var paramId = new SqlParameter { ParameterName = "@id", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                command.Parameters.Add(paramId);
                command.ExecuteNonQuery();
                return (int)paramId.Value;
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"DELETE Product
                             WHERE Id LIKE @id";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"SELECT * FROM Product";
                var command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        var product = new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3)
                        };

                        products.Add(product);
                    }
                }

            }

            return products;
        }

        public Product GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Product product)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"UPDATE Product
                              SET Name = @name , Price = @price , Quantity = @quantity
                              WHERE Id = @id";

                var command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@name", product.Name));
                command.Parameters.Add(new SqlParameter("@price", product.Price));
                command.Parameters.Add(new SqlParameter("@quantity", product.Quantity));
                command.Parameters.Add(new SqlParameter("@id", product.Id));

                command.ExecuteNonQuery();
            }
        }
         
    }
}
