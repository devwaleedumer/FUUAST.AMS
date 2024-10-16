namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record CreateApplicantPSInfoResponse
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public string FatherName { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string Cnic { get; set; } = default!;
        public DateTime Dob { get; set; }
        public string Gender { get; set; } = default!;
        public string MobileNo { get; set; } = default!;
        public string Domicile { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string Religion { get; set; } = default!;
        public string Bloodgroup { get; set; } = default!;
        public int? PostalCode { get; set; }
        public string PermanentAddress { get; set; } = default!;
        public string? EmploymentDetails { get; set; }
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public UpdateGuardianResponse? Guardian { get; set; }
        public UpdateEmergencyContactResponse? EmergencyContact { get; set; }
    }
}
