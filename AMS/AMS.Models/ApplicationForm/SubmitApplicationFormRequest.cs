namespace AMS.MODELS.ApplicationForm
{
    public record SubmitApplicationFormRequest(bool InfoConsent,string HeardAboutUniFrom, string ExpelledFromUni,List<SubmitApplicationAppliedProgramsRequest> ProgramsApplied);
    public record SubmitApplicationAppliedProgramsRequest (int DepartmentId, int TimeShiftId);
}
