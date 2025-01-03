using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record ApplicantPSInfoResponse
    {
        public int Id { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string FatherName { get; set; } = default!;
        public string Cnic { get; set; } = default!;
        public DateOnly? Dob { get; set; } 
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
        public GuardianResponse Guardian { get; set; } = default!;
        public EmergencyContactResponse EmergencyContact { get; set; } = default!;
    }
}
