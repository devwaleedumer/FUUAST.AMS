using AMS.MODELS.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.IDataService
{
    public interface IProgramService
    {
        Task<List<ProgramResponse>> GetAllPrograms(CancellationToken ct);
        Task<ProgramResponse> GetProgramByApplicantId(int applicantId, CancellationToken ct);

    }
}
