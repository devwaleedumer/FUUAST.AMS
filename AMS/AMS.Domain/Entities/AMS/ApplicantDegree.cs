
using AMS.DOMAIN.Entities.Lookups;

namespace AMS.DOMAIN.Entities.AMS
{
    public class ApplicantDegree
    {
        public int Id { get; set; }
        public  string BoardOrUniversityName { get; set; }
        public int PassingYear { get; set; }
        public string? Subject { get; set; }
        public string RollNo { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }

        public int? ApplicantId { get; set; }
        public int? DegreeGroupId { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual DegreeGroup? DegreeGroup { get; set; }


        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
