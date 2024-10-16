using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
namespace AMS.DOMAIN.Entities.Lookups
{
    public class Program : IBaseEntity
    {
        public Program()
        {
            ProgramDepartments = new HashSet<ProgramDepartment>();
            ApplicationForms = new HashSet<ApplicationForm>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ProgramTypeId { get; set; }
        public virtual ProgramType? ProgramType { get; set; }
        public virtual ICollection<ProgramDepartment>? ProgramDepartments { get; set; }
        public virtual ICollection<ApplicationForm>? ApplicationForms { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
