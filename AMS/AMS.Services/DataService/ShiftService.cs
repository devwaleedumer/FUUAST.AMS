using AMS.DATA;
using AMS.MODELS.Shift;
using AMS.SERVICES.IDataService;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AMS.SERVICES.DataService
{
    public class ShiftService(AMSContext context) : IShiftService
    {
        private readonly AMSContext _context = context;
        public async Task<List<ShiftResponse>> GetTimeShiftByDepartmentAndProgramId(int departmentId,int programId, CancellationToken ct)
        {
            var result = await _context.ProgramDepartments
                                       .AsNoTracking()
                                       .Where(pd => pd.DepartmentId == departmentId && pd.ProgramId == programId)
                                       .Select(x => x.TimeShift)
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<ShiftResponse>>();
        }
    }
}
