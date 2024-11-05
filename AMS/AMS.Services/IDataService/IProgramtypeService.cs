using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.ProgramType;
using AMS.MODELS.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.IDataService
{
    public interface IProgramtypeService
    {
        Task<List<ProgramtypeResponse>> GetAllProgramType(CancellationToken ct);
        Task DeleteProgramType(int id, CancellationToken ct);
        Task<UpdateProgramTypeResponse> UpdateProgramType(UpdateProgramTypeRequest Request,
           CancellationToken ct);

        Task<CreateProgramTypeResponse> CreateProgramType(CreateProgramTypeRequest Request,
           CancellationToken ct);
        Task<PaginationResponse<ProgramtypeResponse>> GetProgramtypeByFilter(LazyLoadEvent request, CancellationToken ct);
    }
}
