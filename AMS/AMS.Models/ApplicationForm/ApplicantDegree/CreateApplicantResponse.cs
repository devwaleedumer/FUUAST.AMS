using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.ApplicantDegree
{
     public record CreateApplicantDegreeResponse(int Id,string BoardOrUniversityName, int PassingYear, string Subject, string RollNo, int TotalMarks, int ObtainedMarks, int DegreeGroupId);
    
}
