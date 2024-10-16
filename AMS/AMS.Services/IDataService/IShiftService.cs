using AMS.MODELS.Shift;

namespace AMS.SERVICES.IDataService
{
    public interface IShiftService
    {
        Task<List<ShiftResponse>> GetTimeShiftByDepartmentAndProgramId(int departmentId,int programId, CancellationToken ct);
    }
}