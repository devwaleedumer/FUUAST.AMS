namespace AMS.MODELS.ApplicationForms.Ug
{
    public class ApplicationRequest
    {
        public required ApplicantRequest Applicant { get; set; }
        public required GuardianRequest Guardian { get; set; }
        public required EmergencyContactRequest EmergencyContact { get; set; }
        public required List<DegreeRequest> Degree { get; set; }
        public required List<ProgramApplyRequest> ProgramApply { get; set; }
        public required ApplicationFormRequest ApplicationForms { get; set; }
}
    public class ApplicantRequest
    {
        //public int ApplicantId { get; set; }
        public int ApplicationUserId { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string Domicile { get; set; }
        public string Province { get; set; }
        public string Religion { get; set; }
        public string Bloodgroup { get; set; }
        public string HeardAboutUniFrom { get; set; }
        public int? PostalCode { get; set; }
        public string PermanentAddress { get; set; }

        public string EmploymentDetails { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    public class GuardianRequest
    {
        public int ApplicantId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string ContactNo { get; set; }
        public string MobileNumber { get; set; }
        public string PermanentAddress { get; set; }
    }
    public class EmergencyContactRequest
    {
        public int ApplicantId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string ContactNo { get; set; }
        public string PermanentAddress { get; set; }

    }
    public class DegreeRequest
    {
        public int ApplicantId { get; set; }
        public string BoardOrUniversityName { get; set; }
        public int PassingYear { get; set; }
        public string Subject { get; set; }
        public string RollNo { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
    }
    public class ProgramApplyRequest
    {
        public int ApplicationFormId { get; set; }
        public int DepartmentId { get; set; }
        public int TimeShiftId { get; set; }
        public int ProgramId { get; set; }
        public int PreferenceNo { get; set; }
    }
    public class ApplicationFormRequest
    {
        public int SessionId { get; set; }
        public bool? InfoConsent { get; set; }
        public int? StatusEid { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public bool IsSubmitted { get; set; }
        public bool HaveValidTest { get; set; }
        public int? ApplicantId { get; set; }
    }
}
