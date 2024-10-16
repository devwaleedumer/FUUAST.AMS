using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.ApplicantDegree
{
    public record EditApplicantDegreeListRequest(List<EditApplicantDegreeRequest> Degrees =default!);
}
