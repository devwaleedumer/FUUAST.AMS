using AMS.MODELS.Department;
using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;

namespace AMS.SERVICES.IDataService
{
    public interface IDepartmentService
    {
        Task<List<DeparmentResponse>> GetDepartmentsByFacultyId(int facultyId, int programId, CancellationToken ct);
        Task<List<DeparmentResponse>> GetAllDepartment(CancellationToken ct);
        Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest Request,CancellationToken ct);
        Task<UpdateDepartmentResponse> UpdateDepartment(UpdateDepartmentRequest Request, CancellationToken ct);
        Task DeleteDepartment(int id, CancellationToken ct);
        Task<PaginationResponse<DeparmentResponse>> GetDepartmentByFilter(LazyLoadEvent request, CancellationToken ct);
        Task<List<DeparmentResponse>> GetDepartmentsByProgramId(int programId, CancellationToken ct);

    }
}