using System.Data.Linq.Mapping;

namespace MapLing
{
    [Table(Name = "Continents")]
    public class Continent
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
    }
}
