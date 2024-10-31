using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;

namespace AMS.SERVICES.IDataService
{
    public interface IFacultyService
    {
        Task<List<FaculityResponse>> GetAllFaculties(CancellationToken ct);
        Task<PaginationResponse<FaculityResponse>> GetFacultiesByFilter(LazyLoadEvent request, CancellationToken ct);

    }
}