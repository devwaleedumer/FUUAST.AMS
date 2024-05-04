using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;


namespace AMS.DOMAIN.Entities.Lookups
{
    public class AdmissionSession : IBaseEntity
    {
        public AdmissionSession()
        {
            ApplicationForms = new HashSet<ApplicationForm>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AcademicYearId { get; set; }
        public virtual ICollection<ApplicationForm>? ApplicationForms { get; set; }
        public AcademicYear? AcademicYear { get; set; }
        public virtual Program? Program { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
