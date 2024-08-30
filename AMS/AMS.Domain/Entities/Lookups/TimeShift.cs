using AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class TimeShift : IBaseEntity
    {
        public TimeShift()
        {
            Departments = new HashSet<Department>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
