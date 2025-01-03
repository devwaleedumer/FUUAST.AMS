using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record UpdateApplicantPSInfoRequest
    {
        public int Id { get; set; }
        public FileRequest? ImageRequest { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string Domicile { get; set; }
        public string Province { get; set; }
        public string Religion { get; set; }
        public string BloodGroup { get; set; }
        public int PostalCode { get; set; }
        public string PermanentAddress { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public UpdateGuardianRequest Guardian { get; set; }
        public UpdateEmergencyContactRequest EmergencyContact { get; set; }
    }
}
