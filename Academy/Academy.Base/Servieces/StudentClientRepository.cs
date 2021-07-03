using Academy.Base.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Academy.Base.Servieces
{
    public class StudentClientRepository: IStudentClientRepository
    {
        private readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Academy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public AuthenticationUser Authentication(string login, string password, Func<string, string, Student> action)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    Student student = action.Invoke(login, password);
                    AuthenticationUser authentication = new AuthenticationUser();
                    authentication.student = student;

                    var query = @$"INSERT INTO AuthenticationUsers(Token, StudentId)
                                   VALUES (NEWID(), @id);
                                   SELECT @token = Token FROM AuthenticationUsers WHERE StudentId = @id";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.Add(new SqlParameter("@id", student.Id));
                    var paramToken = new SqlParameter { ParameterName = "@token", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, Direction = System.Data.ParameterDirection.Output };
                    command.Parameters.Add(paramToken);
                    command.ExecuteNonQuery();
                    authentication.Token = (Guid)paramToken.Value;
                    return authentication;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw new Exception("invalid Authentication!");
                }
            }
        }
        public AuthenticationUser Authentication(Guid token)
        {
            AuthenticationUser authentication = new AuthenticationUser();
            authentication.Token = token;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = "SELECT @id = StudentId FROM AuthenticationUsers WHERE Token = @token";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@token", token));
                var paramId = new SqlParameter { ParameterName = "@id", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                command.Parameters.Add(paramId);
                command.ExecuteNonQuery();
                if (paramId.Value != null)
                {
                    authentication.student = GetById((int)paramId.Value);
                }
                else throw new Exception("Invalid token!");
                return authentication;
            }
        }
        public void LogOut(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = "DELETE FROM AuthenticationUsers WHERE StudentId = @id";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.ExecuteNonQuery();
            }
        }
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
                        //item.PasswordHash = reader.GetString(6);
                        //item.Salt = reader.GetString(7);
                        item.LastModify = (DateTime)reader.GetValue(8);
                    }
                }
                else { throw new Exception("Student not found!"); }
                return item;
            }
        }
        public Student Login(string Login, string Password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query1 = "SELECT Id, PasswordHash, Salt FROM Student WHERE UserName = @login";
                var commad1 = new SqlCommand(query1, connection);
                commad1.Parameters.Add(new SqlParameter("@login", Login));
                var reader = commad1.ExecuteReader();
                int id = -1;
                string passwordHash = "";
                string salt = "";
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        passwordHash = reader.GetString(1);
                        salt = reader.GetString(2);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid login");
                }
                var passwordAndSalt = Password + salt;
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                var sha256 = SHA256.Create();
                byte[] secret = sha256.ComputeHash(unicodeEncoding.GetBytes(passwordAndSalt));//ProtectedData.Protect(unicodeEncoding.GetBytes(passwordAndSalt), null, DataProtectionScope.CurrentUser);
                string cryptedPassword = Convert.ToBase64String(secret);
                if (cryptedPassword.Equals(passwordHash))
                {
                    return GetById(id);
                }
                else { throw new Exception("Invalid password"); }
            }
        }
         

        public Student Registation(string Login, string Password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var query = @"UPDATE Student
                              SET PasswordHash = @password,
                              Salt = @salt
                              WHERE PasswordHash IS NULL AND UserName = @login;
                              SELECT @Id = Id FROM Student WHERE UserName = @login";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@login", Login));
                var salt = Guid.NewGuid().ToString();
                command.Parameters.Add(new SqlParameter("@salt", salt));
                var PasswordAndSalt = Password + salt;

                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                var sha256 = SHA256.Create();
                byte[] secret = sha256.ComputeHash(unicodeEncoding.GetBytes(PasswordAndSalt));//ProtectedData.Protect(unicodeEncoding.GetBytes(passwordAndSalt), null, DataProtectionScope.CurrentUser);
                string cryptedPassword = Convert.ToBase64String(secret);
                command.Parameters.Add(new SqlParameter("@password", cryptedPassword));
                var paramId = new SqlParameter { ParameterName = "@Id", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                command.Parameters.Add(paramId);
                var result = command.ExecuteNonQuery();
                return GetById((int)paramId.Value);
            }
        }
    }
}
