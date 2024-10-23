using AMS.DOMAIN.Base;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class Faculity : IBaseEntity
    {
        public Faculity()
        {
            Departments = new HashSet<Department>();
            ProgramDepartments = new HashSet<ProgramDepartment>();

        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<Department>? Departments { get; set; }
        public virtual ICollection<ProgramDepartment>? ProgramDepartments { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
