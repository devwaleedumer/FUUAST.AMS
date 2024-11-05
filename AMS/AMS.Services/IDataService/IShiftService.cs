using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Shift;

namespace AMS.SERVICES.IDataService
{
    public interface IShiftService
    {
        Task<List<ShiftResponse>> GetTimeShiftByDepartmentAndProgramId(int departmentId,int programId, CancellationToken ct);
        Task <List<ShiftResponse>> GetAllShift(CancellationToken ct);
        Task DeleteShift(int id, CancellationToken ct);
        Task<UpdateShiftResponse> UpdateShift(UpdateShiftRequest shiftRequest,
           CancellationToken ct);

        Task<CreateShiftResponse> CreateFaculty(CreateShiftRequest shiftRequest,
           CancellationToken ct);
        Task<PaginationResponse<ShiftResponse>> GetShiftByFilter(LazyLoadEvent request, CancellationToken ct);
    }
}