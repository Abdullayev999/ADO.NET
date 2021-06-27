using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLing
{
    [Table(Name = "Cities")]
    public class City
    {
        [Column(IsPrimaryKey =true,IsDbGenerated =true)]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public int CountPeople { get; set; }
    }
}
