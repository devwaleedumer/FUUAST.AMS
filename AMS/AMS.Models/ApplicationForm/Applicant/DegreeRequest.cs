namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record DegreeRequest
    {
        public string BoardOrUniversityName { get; set; } = default!;
        public int PassingYear { get; set; }
        public string Subject { get; set; } = default!;
        public string RollNo { get; set; } = default!;
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
    }
}
