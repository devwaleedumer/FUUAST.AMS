using AMS.DATA;
using AMS.MODELS.Department;
using AMS.SERVICES.IDataService;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace AMS.SERVICES.DataService
{
    public class DepartmentService(AMSContext context) : IDepartmentService
    {
        private readonly AMSContext _context = context;
        public async Task<List<DeparmentResponse>> GetDepartmentsByFacultyId(int facultyId,int programId, CancellationToken ct)
        {
            var result = await _context.ProgramDepartments
                                       .Include((x) => x.Department)
                                       .AsNoTracking()
                                       .Where(programDepartment => programDepartment.Department!.FaculityId == facultyId && programDepartment.ProgramId == programId )
                                       .Select(d  => d.Department)
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<DeparmentResponse>>();
        }
    }
}
