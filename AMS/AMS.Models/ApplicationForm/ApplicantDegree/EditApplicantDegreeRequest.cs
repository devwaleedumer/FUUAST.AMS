namespace AMS.MODELS.ApplicationForm.ApplicantDegree
{
    public record EditApplicantDegreeRequest(int Id, int ApplicantId, string BoardOrUniversityName, int PassingYear, string Subject, string RollNo, int TotalMarks, int ObtainedMarks, int DegreeGroupId);
}
