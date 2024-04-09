using AMS.DOMAIN.Entities.AMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class Program
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ProgramCode { get; set; }
        public int ProgramTypeId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsProgramOffered { get; set; }
        public decimal Duration { get; set; }
        public int ShiftEid { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ProgramType? ProgramType { get; set; }
        public virtual ProgramApplied? ProgramApplied { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
