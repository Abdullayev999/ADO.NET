using System;
using System.Data.Linq;
using System.Linq;
using System.Threading;

namespace MapLing
{
    class Program
    {
        static void Main(string[] args)
        {

            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Map;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
             
            do
            {
                //Esli ruskiy wrift ne otkroyetsa

                //Console.WriteLine("\n 1  - Show all country information"
                //               + "\n 2  - Display country names"
                //               + "\n 3  - Show name of capitals"
                //               + "\n 4  - Display the name of major cities in a specific country"
                //               + "\n 5  - Display the name of capitals with more than 5 million inhabitants"
                //               + "\n 6  - Display the name of all European countries"
                //               + "\n 7  - Display the name of countries with an area larger than a specific number"
                //               + "\n 8  - Show all capitals with letters <<a>>, <<p>> in their names"
                //               + "\n 9  - Show all capitals with names starting with the letter <<k>>"
                //               + "\n 10 - Display the name of the countries whose area is in the specified range"
                //               + "\n 11 - Display the name of countries with more than a specified number of inhabitants"
                //               + "\n 12 - Show top - 5 countries by area"
                //               + "\n 13 - Show top - 5 capitals by population"
                //               + "\n 14 - Show the country with the largest area"
                //               + "\n 15 - Show the capital with the most inhabitants"
                //               + "\n 16 - Show the country with the smallest square in Europe"
                //               + "\n 17 - Show the average area of countries in Europe"
                //               + "\n 18 - Show top - 3 cities by number of inhabitants for a specific country"
                //               + "\n 19 - Show total number of countries"
                //               + "\n 20 - Show the part of the world with the maximum number of countries"
                //               + "\n 21 - Show the number of countries in each part of the world"
                //               + "\n 22 - Exit\n"); 

                Console.WriteLine("\n1  - Отобразить всю информацию о странах"
                                + "\n2  - Отобразить название стран"
                                + "\n3  - Отобразить название столиц"
                                + "\n4  - Отобразить название крупных городов конкретной страны"
                                + "\n5  - Отобразить название столиц с количеством жителей больше пяти миллионов"
                                + "\n6  - Отобразить название всех европейских стран"
                                + "\n7  - Отобразить название стран с площадью большей конкретного числа"
                                + "\n8  - Отобразить все столицы, у которых в названии есть буквы a, p"
                                + "\n9  - Отобразить все столицы, у которых название начинается с буквы k"
                                + "\n10 - Отобразить название стран, у которых площадь находится в указанном диапазоне"
                                + "\n11 - Отобразить название стран, у которых количество жителей больше указанного числа"
                                + "\n12 - Показать топ - 5 стран по площади"
                                + "\n13 - Показать топ - 5 столиц по количеству жителей"
                                + "\n14 - Показать страну с самой большой площадью"
                                + "\n15 - Показать столицу с самым большим количеством жителей"
                                + "\n16 - Показать страну с самой маленькой площадью в Европе"
                                + "\n17 - Показать среднюю площадь стран в Европе"
                                + "\n18 - Показать топ - 3 городов по количеству жителей для конкретной страны"
                                + "\n19 - Показать общее количество стран"
                                + "\n20 - Показать часть света с максимальным количеством стран"
                                + "\n21 - Показать количество стран в каждой части света"
                                + "\n22 - Exit\n");


                int.TryParse(Console.ReadLine(), out int action);
                Console.Clear();
                if (action == 1)
                {
                    using var db = new DataContext(connectionString); 
                       
                    var query =from country in db.GetTable<Country>()
                               join continent in db.GetTable<Continent>() on country.ContinentId equals continent.Id 
                               join city in db.GetTable<City>() on country.CapitalId equals city.Id
                               select new
                              {
                                 Country = country.Name,Continent = continent.Name,
                                 Area = country.Area,Capital = city.Name, CountPeople = country.CountPeople
                              };
                     

                    foreach (var country in query)
                    {
                        Console.WriteLine(country.Country.PadRight(12,' ')+ country.Capital.PadRight(12, ' ') + 
                        country.Continent.PadRight(12, ' ') + country.CountPeople+"\t" + country.Country.PadRight(12, ' '));
                    }

                }
                else if (action == 2)
                {
                    using var db = new DataContext(connectionString);
                    var query = db.GetTable<Country>().Select(i => i.Name).Distinct();

                    foreach (var item in query)
                    {
                        Console.WriteLine(item);
                    }
                }
                else if (action == 3)
                {
                    using var db = new DataContext(connectionString);

                    var query = from country in db.GetTable<Country>()
                                join city in db.GetTable<City>() on country.CapitalId equals city.Id
                                select new { Country = country.Name,Capital = city.Name };

                    Console.WriteLine("1 Method\n");
                    Console.WriteLine("----------------------------------\n\n");


                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Country.PadRight(12, ' ') + item.Capital);
                    }

                    Thread.Sleep(2000);

                    Console.WriteLine("\n\n2 Method\n");
                    Console.WriteLine("----------------------------------\n");
                    
                    query = db.GetTable<Country>()
                                .Join(db.GetTable<City>(), country => country.CapitalId, city => city.Id, 
                                (country, city) => new { Country = country.Name, Capital = city.Name });
                        


                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Country.PadRight(12,' ') + item.Capital);
                    }
                }
                else if (action == 4)
                {
                    try
                    {
                        using var db = new DataContext(connectionString);
                        Console.Write("Enter your country to find out the largest city : ");
                        var CountryName = Console.ReadLine(); 
                         

                      var  query1 = (from country in db.GetTable<Country>()
                                 join countryByCity in db.GetTable<CountryByCity>() on country.Id equals countryByCity.CountryId
                                 join city in db.GetTable<City>() on countryByCity.CityId equals city.Id
                                 where country.Name.Equals(CountryName)
                                 orderby city.CountPeople descending
                                 select new { Country = country.Name, City = city.Name, CountPeople = city.CountPeople }).Take(1).FirstOrDefault();

                        Console.WriteLine(query1.Country.PadRight(12, ' ') + query1.City.PadRight(12, ' ') + query1.CountPeople);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (action == 5)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                        .Join(db.GetTable<City>(),country=>country.CapitalId,city=>city.Id,
                       (country, city) => new { Capital = city.Name,CountPeople = city.CountPeople })
                        .Where(city=>city.CountPeople>5000000);

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Capital.PadRight(12,' ')+item.CountPeople);
                    }
                }
                else if (action == 6)
                {
                    using var db =new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                                .Join(db.GetTable<Continent>(), country => country.ContinentId, continent => continent.Id,
                                (country, continent) => new { country=country.Name, continent=continent.Name})
                                .Where(continent=>continent.continent.Equals("Europe"));

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.country);
                    }
                }
                else if (action == 7) 
                {
                    using var db = new DataContext(connectionString);

                    Console.Write("Enter area : ");
                    double.TryParse(Console.ReadLine(), out double area);

                    var query = db.GetTable<Country>().Where(country => country.Area > area);

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name.PadRight(12,' ')+item.Area);
                    }
                }
                else if (action == 8)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                                .Join(db.GetTable<City>(), country => country.CapitalId, city => city.Id,
                                (country, city) => new { city.Name })
                                .Where(city => city.Name.ToLower().Contains("a") || city.Name.ToLower().Contains("p"));// && у меня нету поэт или проверил

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
                else if (action == 9)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                                 .Join(db.GetTable<City>(), country => country.CapitalId, city => city.Id,
                                 (country, city) => new { city.Name })
                                 .Where(city => city.Name.ToLower().Substring(0,1)=="k");

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
                else if (action == 10)
                {
                    using var db = new DataContext(connectionString);

                    Console.Write("Enter deapazon area\nStart : ");
                    int.TryParse(Console.ReadLine(), out int start);
                     
                    Console.Write("End : ");
                    int.TryParse(Console.ReadLine(), out int end);

                    var query = db.GetTable<Country>().Where(country => country.Area > start && country.Area < end).OrderByDescending(country=>country.Area);


                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name.PadRight(12,' ')+item.Area);
                    }
                }
                else if (action == 11)
                {
                    using var db = new DataContext(connectionString);
                    Console.Write("Enter count people : ");
                    int.TryParse(Console.ReadLine(), out int countPeople);
                    var query = db.GetTable<Country>().Where(country => country.CountPeople > countPeople);

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name.PadRight(12,' ')+item.CountPeople);
                    }
                }
                else if (action == 12)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>().OrderByDescending(country => country.Area).Take(5).Select((country)=>new { country.Name,country.Area});

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name.PadRight(12,' ')+item.Area);
                    }
                }
                else if (action == 13)
                {
                    using var db = new DataContext(connectionString);

                    var query = (from country in db.GetTable<Country>()
                                 join city in db.GetTable<City>() on country.CapitalId equals city.Id
                                 orderby city.CountPeople descending
                                 select new { Country =  country.Name,  City = city.Name,CountPeople =city.CountPeople}).Take(5);

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Country.PadRight(12,' ')+item.City.PadRight(12,' ')+item.CountPeople);
                    }
                }
                else if (action == 14)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>().OrderByDescending(country => country.Area).Take(1).Select((country) => new { country.Name ,country.Area});

                    Console.WriteLine(query.First().Name.PadRight(12,' ')+ query.First().Area); 
                }
                else if (action == 15)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                                 .Join(db.GetTable<City>(), country => country.CapitalId, city => city.Id,
                                  (country, city) => new { Capital = city.Name, CountPeople = city.CountPeople })
                                 .OrderByDescending(city => city.CountPeople).Take(1);
                     
                   Console.WriteLine(query.First().Capital.PadRight(12,' ')+ query.First().CountPeople);
                
                }
                else if (action == 16)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                                .Join(db.GetTable<Continent>(), country => country.ContinentId, continent => continent.Id,
                                (country, continent) => new { Country = country.Name, Area = country.Area, Continent = continent.Name })
                                .Where(country => country.Continent.Equals("Europe")).OrderBy(country => country.Area).Take(1);//.Select(item=> item.Country);
                                // вывожу не только название чтобы было ясно 
                    
                    Console.WriteLine(query.First().Country.PadRight(12,' ')+ query.First().Continent.PadRight(12, ' ') + query.First().Area);
                    
                }
                else if (action == 17)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>()
                                .Join(db.GetTable<Continent>(), country => country.ContinentId, continent => continent.Id,
                                (country, continent) => new { Country = country.Name, Area = country.Area, Continent = continent.Name })
                                .Where(country => country.Continent.Equals("Europe")).Average(item=>item.Area);

                    Console.WriteLine("Avg area : " + query);
                }
                else if (action == 18)
                {
                    using var db = new DataContext(connectionString);
                    Console.Write("Enter country : ");
                    var countryName = Console.ReadLine();

                    var query = (from country in db.GetTable<Country>()
                                join countryByCity in db.GetTable<CountryByCity>() on country.Id equals countryByCity.CountryId
                                join city in db.GetTable<City>() on countryByCity.CityId equals city.Id
                                 orderby city.CountPeople descending
                                 where country.Name.Equals(countryName) 
                                select new { Country = country.Name, City = city.Name, CountPeople = city.CountPeople }).Take(3);

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Country.PadRight(12,' ')+item.City.PadRight(12,' ')+item.CountPeople);
                    }

                }
                else if (action == 19)
                {
                    using var db = new DataContext(connectionString);

                    var query = db.GetTable<Country>().Count();
                    Console.WriteLine("All countries count : " + query);
                }
                else if (action == 20)
                {
                    using var db = new DataContext(connectionString);



                    var query = (from countr in db.GetTable<Country>()
                                 join cont in db.GetTable<Continent>()
                                 on countr.ContinentId equals cont.Id
                                 group cont by cont.Name into r
                                 select new
                                 {
                                     Continent = r.Key,
                                     Count = r.Count()
                                 }).OrderByDescending(i=>i.Count).Take(1); 

                    Console.WriteLine("Continent : " + query.First().Continent   + " " + query.First().Count + " countries");

                }
                else if (action == 21)
                {
                    using var db = new DataContext(connectionString);

                    var query = (from countr in db.GetTable<Country>()
                                 join cont in db.GetTable<Continent>()
                                 on countr.ContinentId equals cont.Id
                                 group cont by cont.Name into r
                                 select new
                                 {
                                     Continent = r.Key,
                                     Count = r.Count()
                                 }).OrderByDescending(i => i.Count);

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Continent.PadRight(15,' ')  + " " + item.Count + " countries");
                    }
                  

                }
                else if (action == 22)
                {
                    return;
                }

                Console.ReadKey();
                Console.Clear();
            } while (true);
             
        }
    }
     
}
