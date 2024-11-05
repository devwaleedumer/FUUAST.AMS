using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Program;
using AMS.MODELS.ProgramType;
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
        Task<CreateProgramResponse> CreateProgram(CreateProgramRequest Request, CancellationToken ct);
        Task<UpdateProgramResponse> UpdateProgram(UpdateProgramRequest request, CancellationToken ct);
        Task DeleteProgram(int id, CancellationToken ct);
        Task<PaginationResponse<ProgramResponse>> GetProgramByFilter(LazyLoadEvent request, CancellationToken ct);

    }
}
