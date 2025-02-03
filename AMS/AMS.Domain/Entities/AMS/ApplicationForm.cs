using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.Lookups;

namespace AMS.DOMAIN.Entities.AMS
{
    /// <summary>
    ///  ApplicationForm Contains Data related to Application Form
    /// </summary>
    public class ApplicationForm : IBaseEntity
    {
        public ApplicationForm()
        {
            ProgramsApplied = new HashSet<ProgramApplied>();
        }
        public string? Remarks { get; set; }
        public int Id { get; set; }
        public bool? InfoConsent { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public bool IsSubmitted { get; set; }
        public virtual Applicant? Applicant { get; set; }
        public virtual FeeChallan? FeeChallan { get; set; }
        // List of All Applied Programs 
        public virtual ICollection<ProgramApplied>? ProgramsApplied { get; set; }
        public virtual AdmissionSession? Session { get; set; }
        public virtual Program? Program { get; set; }
        public virtual MeritListDetails? MeritListDetails { get; set; }
        public int? SessionId { get; set; }
        public int? ApplicantId { get; set; }
        public int? ProgramId { get; set; }
        public int? VerificationStatusEid { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
