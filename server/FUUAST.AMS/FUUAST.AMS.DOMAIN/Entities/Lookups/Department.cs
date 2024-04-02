using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Entities.Lookups
{
    public class Department
    {
        public Department()
        {
            Programs = new HashSet<Program>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Program> Programs { get; set; }
    }
}
