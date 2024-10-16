namespace AMS.MODELS.ApplicationForm.ApplicantDegree
{
    public record CreateApplicantDegreeRequest(string BoardOrUniversityName,int PassingYear, string Subject, string RollNo,int TotalMarks,int ObtainedMarks,int DegreeGroupId);
}
