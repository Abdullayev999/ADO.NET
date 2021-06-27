using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Products
{

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Colorie { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Name}".PadRight(15,' ')  +$"{Type}".PadRight(12, ' ') +$"{Color}".PadRight(12, ' ') + $"{Colorie}";
        }


        public string Print()
        {
            return $"{Color}".ToString().PadRight(10, ' ') + $"{Count}";
        }
    }
     
    class Program
    {
        static void Main(string[] args)
        {
            List<Product> ProductList = new List<Product>();
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VegetablesAndFruits;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            do
            {
                Console.Clear();
                Console.WriteLine("Make a choice\n"+
                "\n1  - Отображение всей информации из таблицы с овощами и фруктами"+
                "\n2  - Отображение всех названий овощей и фруктов"+
                "\n3  - Отображение всех цветов"+
                "\n4  - Показать максимальную калорийность"+
                "\n5  - Показать минимальную калорийность"+
                "\n6  - Показать среднюю калорийность"+
                "\n7  - Показать количество овощей"+
                "\n8  - Показать количество фруктов"+
                "\n9  - Показать количество овощей и фруктов заданного цвета"+
                "\n10 - Показать количество овощей фруктов каждого цвета"+
                "\n11 - Показать овощи и фрукты с калорийностью ниже указанной"+
                "\n12 - Показать овощи и фрукты с калорийностью выше указанной"+
                "\n13 - Показать овощи и фрукты с калорийностью в указанном диапазоне"+
                "\n14 - Показать все овощи и фрукты, у которых цвет желтый или красный."+
                "\n15 - Exit");

                int.TryParse(Console.ReadLine(), out int choose);
                Console.Clear(); 

                if (choose == 1)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT * FROM Products";
                        var command = new SqlCommand(query, connection);
                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int index = 0;
                                var id = reader.GetInt32(index++);
                                var name = reader.GetString(index++);
                                var type = reader.GetString(index++);
                                var calorie = reader.GetDouble(index++);
                                var color = reader.GetString(index);

                                var product = new Product { Id = id, Name = name, Type = type, Colorie = calorie, Color = color };
                                ProductList.Add(product);
                            }

                            foreach (var item in ProductList)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        } 
                        Console.ReadKey();
                    } 
                }
                else if (choose == 2)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"SELECT DISTINCT Name FROM Products";

                        var command = new SqlCommand(query, connection);
                        var reader = command.ExecuteReader();

                        var list = new List<string>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var name = reader.GetString(0);
                                list.Add(name);
                            }
                             
                            foreach (var item in list)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        } 
                        Console.ReadKey();
                    }
                }
                else if (choose == 3)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT DISTINCT Color FROM Products";

                        var command = new SqlCommand(query, connection);
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            var list = new List<string>();

                            while (reader.Read())
                            {
                                list.Add(reader.GetString(0));
                            }

                            foreach (var item in list)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        } 
                        Console.ReadKey();
                    }
                }else if(choose == 4)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT MAX(Calorie) FROM Products";

                        var command =new SqlCommand(query, connection);
                        var reader = (double)command.ExecuteScalar();

                        Console.WriteLine($"Max calorie : {reader}");
                        Console.ReadKey();
                    }
                }
                else if (choose == 5)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT MIN(Calorie) FROM Products";

                        var command = new SqlCommand(query, connection);

                        var reader =(double)command.ExecuteScalar();
                        Console.WriteLine($"Min calorie : {reader}");
                        Console.ReadKey();
                    }
                }
                else if (choose == 6)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var query = "SELECT AVG(Calorie) FROM Products";

                        var command = new SqlCommand(query, connection);
                        var reader = (double)command.ExecuteScalar(); 
                        Console.WriteLine($"Avg colorie = {reader}");
                        Console.ReadKey(); 
                    }
                }
                else if (choose == 7)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var query = "SELECT COUNT(*) FROM Products WHERE [Type] LIKE 'Vegetable'";
                        var command = new SqlCommand(query, connection);
                        var reader = (int)command.ExecuteScalar();
                        Console.WriteLine($"Count Vegetable : {reader}");
                        Console.ReadKey(); 
                    }
                }
                else if (choose == 8)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var query = "SELECT COUNT(*) FROM Products WHERE [Type] LIKE 'Fruit'";
                        var command = new SqlCommand(query, connection);
                        var reader =(int)command.ExecuteScalar();

                        Console.WriteLine($"Count Fruit : {reader}");
                        Console.ReadKey();
                    }
                }
                else if (choose == 9)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open(); 
                        Console.Write("Enter color : ");
                        var color = Console.ReadLine();

                        var query = $"SELECT COUNT(*) FROM Products WHERE Color LIKE @color";
                        var command = new SqlCommand(query,connection);

                        var colorParam = new SqlParameter("@color", color);

                        command.Parameters.Add(colorParam);
                        var reader = (int)command.ExecuteScalar();
                        if (reader>0)
                        {
                            Console.WriteLine($"Products to {color} color : {reader} counts");
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        }
                        Console.ReadKey();
                    }
                }
                else if (choose == 10)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var query = @"SELECT Color , Count(*)
                                      FROM Products
                                      GROUP BY Color";

                        var command = new SqlCommand(query, connection);
                        var rider = command.ExecuteReader();

                        if (rider.HasRows)
                        {
                            List<Product> list = new List<Product>();
                            while (rider.Read())
                            {
                                Product produc = new Product();
                                produc.Color = rider.GetString(0); 
                                produc.Count = rider.GetInt32(1);

                                list.Add(produc);
                            }

                            foreach (var item in list)
                            {
                                Console.WriteLine(item.Print());
                            } 
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        }
                        Console.ReadKey();
                    }
                }
                else if (choose == 11)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        Console.WriteLine("Enter colorie : ");

                        double.TryParse(Console.ReadLine(), out double colorie);

                        string query = @"SELECT *
                                         FROM Products
                                         WHERE Calorie < @colorie";

                        var command = new SqlCommand(query, connection);

                        var param = new SqlParameter("@colorie", colorie);
                        command.Parameters.Add(param);

                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            var products = new List<Product>();

                            while (reader.Read())
                            {
                                var product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Type = reader.GetString(2);
                                product.Colorie = reader.GetDouble(3);
                                product.Color = reader.GetString(4);

                                products.Add(product);
                            }

                            foreach (var item in products)
                            {
                                Console.WriteLine(item);
                            } 
                        } 
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        }
                        Console.ReadKey(); 
                    }
                }
                else if (choose == 12)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        Console.WriteLine("Enter colorie : ");

                        double.TryParse(Console.ReadLine(), out double colorie);

                        string query = @"SELECT *
                                         FROM Products
                                         WHERE Calorie > @colorie";

                        var command = new SqlCommand(query, connection);

                        var param = new SqlParameter("@colorie", colorie);
                        command.Parameters.Add(param);

                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            var products = new List<Product>();

                            while (reader.Read())
                            {
                                var product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Type = reader.GetString(2);
                                product.Colorie = reader.GetDouble(3);
                                product.Color = reader.GetString(4);

                                products.Add(product);
                            }

                            foreach (var item in products)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        }
                        Console.ReadKey();
                    }
                }
                else if (choose == 13)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        Console.Write("Enter Calorie start : ");
                        double.TryParse(Console.ReadLine(),out double start);

                        Console.Write("Enter Calorie end : ");
                        double.TryParse(Console.ReadLine(), out double end);

                        connection.Open();
                        var query = @"SELECT *
                                       FROM Products
                                       WHERE Calorie BETWEEN @start AND @end";

                        var command = new SqlCommand(query, connection);

                        var startParam = new SqlParameter("@start", start);
                        var endParam = new SqlParameter("@end", end);

                        command.Parameters.Add(startParam);
                        command.Parameters.Add(endParam);

                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            var products = new List<Product>();

                            while (reader.Read())
                            {
                                var product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Type = reader.GetString(2);
                                product.Colorie = reader.GetDouble(3);
                                product.Color = reader.GetString(4);

                                products.Add(product);
                            }

                            foreach (var item in products)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        }
                        Console.ReadKey();
                    }
                }
                else if (choose == 14)
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        var query = @"SELECT *
                                      FROM Products
                                      WHERE Color LIKE 'Red' OR Color LIKE 'Yellow'";

                        var command = new SqlCommand(query, connection);
                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            var products = new List<Product>();

                            while (reader.Read())
                            {
                                var product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Type = reader.GetString(2);
                                product.Colorie = reader.GetDouble(3);
                                product.Color = reader.GetString(4);

                                products.Add(product);
                            }

                            foreach (var item in products)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data, not found!");
                        }
                        Console.ReadKey();
                    }
                }
                else if (choose == 15)
                {
                    break;
                }
            } while (true);
             
          
        }
    }
}
