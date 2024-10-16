namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record CreateApplicantPSInfoRequest
    {
        //public int ApplicantId { get; set; }
        public FileRequest ImageRequest { get; set; } = default!;
        public string FatherName { get; set; } = default!;
        public string Cnic { get; set; } = default!;
        public DateTime Dob { get; set; }
        public string Gender { get; set; } = default!;
        public string MobileNo { get; set; } = default!;
        public string Domicile { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string Religion { get; set; } = default!;
        public string BloodGroup { get; set; } = default!;
        public int? PostalCode { get; set; }
        public string PermanentAddress { get; set; } = default!;

        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public CreateGuardianRequest Guardian { get; set; } = default!;
        public CreateEmergencyContactRequest EmergencyContact { get; set; } = default!;
    }
}
