using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm
{
    public record SubmitApplicationFormRequest(bool InfoConsent,string HeardAboutUniFrom, string ExpelledFromUni,List<SubmitApplicationAppliedProgramsRequest> ProgramsApplied);
    public record SubmitApplicationAppliedProgramsRequest (int DepartmentId, int ProgramId,int TimeShiftId);
}
