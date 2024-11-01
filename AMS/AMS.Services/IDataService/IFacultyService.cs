using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;

namespace AMS.SERVICES.IDataService
{
    public interface IFacultyService
    {
        Task<List<FaculityResponse>> GetAllFaculties(CancellationToken ct);
        Task<PaginationResponse<FaculityResponse>> GetFacultiesByFilter(LazyLoadEvent request, CancellationToken ct);
        Task<CreateFacultyResponse> CreateFaculty(CreateFacultyRequest facultyRequest, CancellationToken ct);
        Task<UpdateFacultyResponse> UpdateFaculty(UpdateFacultyRequest facultyRequest, CancellationToken ct);
        Task DeleteFaculty(int facultyId, CancellationToken ct);


    }
}