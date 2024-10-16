using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.Applicant
{
    public record DegreeResponse
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public string BoardOrUniversityName { get; set; } = default!;
        public int PassingYear { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string RollNo { get; set; } = default!;
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
    }
}
