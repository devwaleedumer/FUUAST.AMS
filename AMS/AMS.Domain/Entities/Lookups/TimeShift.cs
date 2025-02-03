using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
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
            ProgramDepartments = new HashSet<ProgramDepartment>();
            ProgramApplied = new HashSet<ProgramApplied>();
            MeritLists = new HashSet<MeritList>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<ProgramDepartment> ProgramDepartments { get; set; }
        public virtual ICollection<MeritList>? MeritLists { get; set; }
        public virtual ICollection<ProgramApplied> ProgramApplied { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
