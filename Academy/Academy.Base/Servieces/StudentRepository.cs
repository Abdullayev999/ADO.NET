using Academy.Base.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Academy.Base.Servieces
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Academy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public int Create(Student student)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Student(FirstName, LastName, BirthDay, UserName,LastModify)
                              VALUES (@firstname, @lastname, @birthday, @username,GETDATE())
                              SELECT @id = SCOPE_IDENTITY()";

                Random random = new Random();
                student.UserName = $"{student.LastName.ToLower().Substring(0, 3)}_{student.FirstName.ToLower().Substring(0, 2)}{random.Next(0, 1000)}";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@firstname", student.FirstName));
                command.Parameters.Add(new SqlParameter("@lastname", student.LastName));
                
                //if (student.BirthDay == null ) student.BirthDay = default;
                command.Parameters.Add(new SqlParameter("@birthday", student.BirthDay));

                command.Parameters.Add(new SqlParameter("@username", student.UserName));
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
                var query = @"DELETE Student
                             WHERE Id LIKE @id";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Student> GetAll()
        {
            var studetns = new List<Student>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"SELECT Id,FirstName,LastName,BirthDay,Coins,Username,PasswordHash,Salt,LastModify
                              FROM Student";
                var command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            BirthDay = reader.GetDateTime(3),
                            Coins = reader.GetInt32(4),
                            UserName = reader.GetString(5),
                            PasswordHash = reader.GetValue(6) as string,
                            Salt = reader.GetValue(7) as string,
                            LastModify =(DateTime)reader.GetValue(8)
                        };

                        studetns.Add(student);
                    }
                }

            }

            return studetns;
        }

        //public DataTable GetAllDataTable()
        //{
        //    using var connection = new SqlConnection(ConnectionString);
        //    var query = "SELECT * FROM Student";

        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
        //    DataSet dataSet = new DataSet();
        //    dataAdapter.Fill(dataSet);
        //    return dataSet.Tables[0];
        //}

        public Student GetById(int id)
        {
            Student item = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"SELECT Id, FirstName, LastName, BirthDay, Coins, UserName, PasswordHash, Salt, LastModify
                              FROM Student WHERE Id = @Id";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@Id", id));
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new Student();
                        item.Id = reader.GetInt32(0);
                        item.FirstName = reader.GetString(1);
                        item.LastName = reader.GetString(2);
                        item.BirthDay = reader.GetDateTime(3);
                        item.Coins = reader.GetInt32(4);
                        item.UserName = reader.GetString(5);
                        item.PasswordHash = reader.GetValue(6) as string;
                        item.Salt = reader.GetValue(7) as string;
                        item.LastModify = (DateTime)reader.GetValue(8);
                    }
                }
                else { throw new Exception("Student not found!"); }
                return item;
            }
        }

        public bool Update(Student student)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"UPDATE Student
                              SET FirstName = @name , LastName = @surname , Coins = @coins ,LastModify = GETDATE()
                              WHERE Id = @id AND LastModify = @LastModify ";

                var command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@name",student.FirstName));
                command.Parameters.Add(new SqlParameter("@surname",student.LastName));
                command.Parameters.Add(new SqlParameter("@coins",student.Coins));
                command.Parameters.Add(new SqlParameter("@id", student.Id));
                command.Parameters.Add(new SqlParameter("@LastModify", student.LastModify));

                int result = command.ExecuteNonQuery();
                return result == 1 ? true : false;
            }
        }
    }
}
