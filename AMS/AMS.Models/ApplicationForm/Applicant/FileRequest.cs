namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record FileRequest
    {
        public string Name { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public string Data { get; set; } = default!;
    }
}