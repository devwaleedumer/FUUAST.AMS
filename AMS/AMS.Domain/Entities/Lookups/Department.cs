using AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class Department : IBaseEntity
    {
        public Department()
        {
            Programs = new HashSet<Program>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<Program>? Programs { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
