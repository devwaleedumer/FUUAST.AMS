using AMS.MODELS.DegreeGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.Applicant
{
    public class ApplicantmanagementResponse
    {

        public int Id { get; set; }
        public  string Fullname { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; } = default!;
        public string Cnic { get; set; } = default!;
        public DateTime Dob { get; set; } = default!;
        public string MobileNo { get; set; } = default!;
        public string PermanentAddress { get; set; } = default!;

        public string BoardOrUniversityName { get; set; } = default!;
        public int PassingYear { get; set; } = default!;
        public string Rollno { get; set; } = default!;
        public int TotalMarks { get; set; } = default!;
        public int ObtainedMarks { get; set; } = default!;


      

    }
 
}
