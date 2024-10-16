namespace AMS.MODELS.ApplicationForm.ApplicantDegree
{
    public record ApplicantDegreeResponse(int Id, string BoardOrUniversityName, int PassingYear, string Subject, string RollNo, int TotalMarks, int ObtainedMarks, int DegreeGroupId, int ApplicantId);

}
