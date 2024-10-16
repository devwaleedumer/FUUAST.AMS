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
        public async Task<List<DeparmentResponse>> GetDepartmentsByFacultyId(int facultyId, CancellationToken ct)
        {
            var result = await _context.Departments
                                       .AsNoTracking()
                                       .Where(department => department.FaculityId == facultyId)
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<DeparmentResponse>>();
        }
    }
}
