using AMS.DOMAIN.Entities.AMS;
namespace AMS.DOMAIN.Entities.Lookups
{
    public class Program
    {
        public Program()
        {
            ProgramsApplied = new HashSet<ProgramApplied>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ProgramTypeId { get; set; }
        public int TimeShiftId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsProgramOffered { get; set; }
        public decimal Duration { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ProgramType? ProgramType { get; set; }
        public virtual ICollection<ProgramApplied>? ProgramsApplied { get; set; }
        public virtual TimeShift? TimeShift { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
