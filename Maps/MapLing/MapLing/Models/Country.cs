using System.Data.Linq.Mapping;

namespace MapLing
{
    [Table(Name = "Countries")]
    public class Country
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public int ContinentId { get; set; }
        [Column]
        public int CapitalId { get; set; }
        [Column]
        public int CountPeople { get; set; }
        [Column]
        public double Area { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
