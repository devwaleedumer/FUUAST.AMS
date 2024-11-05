using AMS.MODELS.Academicyear;
using AMS.MODELS.Filters;
using AMS.MODELS.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.IDataService
{
    public interface IAcademicyearService
    {
        Task<List<AcademicyearResponse>> GetAllAcedamicYear(CancellationToken ct);
        Task<CreateAcademicyearResponse> CreateAcademicyear(CreateAcademicyearRequest Request,
     CancellationToken ct);
        Task<UpdateAcademicyearResponse> UpdateAcademicyear(UpdateAcademicyearRequest Request,
           CancellationToken ct);
        Task DeleteAcademicYear(int id, CancellationToken ct);
        Task<PaginationResponse<AcademicyearResponse>> GetAcademicYearByFilter(LazyLoadEvent request, CancellationToken ct);
    }
}
