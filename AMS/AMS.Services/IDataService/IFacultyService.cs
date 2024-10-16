using AMS.MODELS.Faculity;

namespace AMS.SERVICES.IDataService
{
    public interface IFacultyService
    {
        Task<List<FaculityResponse>> GetAllFaculties(CancellationToken ct);
    }
}