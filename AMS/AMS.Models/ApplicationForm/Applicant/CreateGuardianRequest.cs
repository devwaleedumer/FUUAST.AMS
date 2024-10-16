namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record CreateGuardianRequest
    {
        public string Name { get; set; } = default!;
        public string Relation { get; set; } = default!;
        public string ContactNo { get; set; } = default!;
        public string PermanentAddress { get; set; } = default!;
    }
}
