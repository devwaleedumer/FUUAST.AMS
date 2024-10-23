using AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class ProgramDepartment : IBaseEntity
    {
        public int Id { get; set; }
        public virtual Faculity? Faculity { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Program? Program { get; set; }
        public virtual TimeShift? TimeShift { get; set; }

        public int? FaculityId { get; set; }
        public int? DepartmentId { get; set; }
        public int ProgramId { get; set; }
        public int TimeShiftId { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
