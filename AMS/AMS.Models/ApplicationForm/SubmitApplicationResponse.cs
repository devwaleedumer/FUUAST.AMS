namespace AMS.MODELS.ApplicationForm
{
    public class SubmitApplicationResponse
    {
        public SubmitApplicationResponse()
        {
            ProgramsApplied = new();
            Departments = new();
            Shifts = new();
        }
        public int Id { get; set; }
        public bool InfoConsent { get; set; }
        public int ProgramId { get; set; }
        public string HeardAboutUniFrom { get; set; } = default!;
        public string ExpelledFromUni { get; set; } = default!;
        public List<SubmitApplicationAppliedPrograms> ProgramsApplied { get; set; }
        public List<List<DepartmentOptions>> Departments {  get; set; } 
        public List<List<TimeShiftOptions>> Shifts {  get; set; } 
    }
    
}
