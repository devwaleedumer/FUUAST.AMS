using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
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
            ProgramApplied = new HashSet<ProgramApplied>();
            ProgramDepartments = new HashSet<ProgramDepartment>();
        }
        public int Id { get; set; }
        public  string Name { get; set; }
        public int FaculityId { get; set; }
        public virtual Faculity? Faculity { get; set; }
        public virtual ICollection<ProgramApplied>? ProgramApplied { get; set; }
        public virtual ICollection<ProgramDepartment>? ProgramDepartments { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
