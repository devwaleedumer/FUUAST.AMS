
using AMS.DOMAIN.Entities.Lookups;

namespace AMS.DOMAIN.Entities.AMS
{
    public class ApplicantDegree
    {
        public int Id { get; set; }
        public string? InstituteName { get; set; }
        public string? BorardOrUniversityName { get; set; }
        public DateTime? FromYear { get; set; }
        public DateTime? ToYear { get; set; }
        public string? MajorSubject { get; set; }
        public string? RollNo { get; set; }
        public int GradingTypeEid { get; set; }
        public int ExamTypeEid { get; set; }
        public string? TranscriptUrl { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }
        public decimal? Percentage { get; set; }

        public int? ApplicantId { get; set; }
        public int? DegreeDetailId { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual PreviousDegreeDetail? DegreeDetail { get; set; }
        
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
