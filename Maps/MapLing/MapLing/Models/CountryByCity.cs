using System.Data.Linq.Mapping;

namespace MapLing
{
    [Table(Name = "CountryByCities")]
    public class CountryByCity
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public int CountryId { get; set; }
        [Column]
        public int CityId { get; set; }
    }
}
