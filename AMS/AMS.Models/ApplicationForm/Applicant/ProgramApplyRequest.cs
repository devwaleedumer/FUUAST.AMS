namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record ProgramApplyRequest
    {
        public int ApplicationFormId { get; set; }
        public int DepartmentId { get; set; }
        public int TimeShiftId { get; set; }
        public int ProgramId { get; set; }
        public int PreferenceNo { get; set; }
    }
}
