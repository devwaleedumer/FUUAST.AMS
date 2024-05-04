
using AMS.DOMAIN.Entities.Lookups;

namespace AMS.DOMAIN.Entities.AMS
{
    public class ApplicantDegree
    {
        public int Id { get; set; }
        public string? InstituteName { get; set; }
        public required string BorardOrUniversityName { get; set; }
        public  int FromYear { get; set; }
        public  int ToYear { get; set; }
        public string? MajorSubject { get; set; }
        public string? RollNo { get; set; }
        public string? TranscriptUrl { get; set; }
        public int? TotalMarks { get; set; }
        public int? ObtainedMarks { get; set; }
        public decimal? Percentage { get; set; }

        public int? ApplicantId { get; set; }
        public int? DegreeTypeId { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual DegreeType? DegreeType { get; set; }
        
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
