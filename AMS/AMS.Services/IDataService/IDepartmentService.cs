using AMS.MODELS.Department;

namespace AMS.SERVICES.IDataService
{
    public interface IDepartmentService
    {
        Task<List<DeparmentResponse>> GetDepartmentsByFacultyId(int faculityId, CancellationToken ct);
    }
}