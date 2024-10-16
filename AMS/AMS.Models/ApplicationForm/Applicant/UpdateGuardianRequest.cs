namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record UpdateGuardianRequest
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public string Name { get; set; } = default!;
        public string Relation { get; set; } = default!;
        public string ContactNo { get; set; } = default!;
        public string PermanentAddress { get; set; } = default!;
    }
}
