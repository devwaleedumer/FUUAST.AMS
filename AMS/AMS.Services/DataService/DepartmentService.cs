using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.Department;
using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Program;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace AMS.SERVICES.DataService
{
    public class DepartmentService(AMSContext context) : IDepartmentService
    {
        private readonly AMSContext _context = context;
        public async Task<List<DeparmentResponse>> GetDepartmentsByFacultyId(int facultyId, int programId, CancellationToken ct)
        {
            var result = await _context.ProgramDepartments
                                       .Include((x) => x.Department)
                                       .AsNoTracking()
                                       .Where(programDepartment => programDepartment.Department!.FaculityId == facultyId && programDepartment.ProgramId == programId)
                                       .Select(d => d.Department)
                                       .Distinct()
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<DeparmentResponse>>();


        }

        public async Task<List<DeparmentResponse>> GetAllDepartment(CancellationToken ct)
        {
            var department = await _context.Departments
       .Include(d => d.Faculity).Where(x => x.IsDeleted != true) // Ensure ProgramType is loaded
       .Select(d => new
       {
           d.Id,
           d.Name,
           d.FaculityId,
           Faculity = d.Faculity.Name // Assuming ProgramType has a Name field
       })

       .ToListAsync(ct)
       .ConfigureAwait(false);

            // Map the result to List<ProgramResponse>
            return department.Adapt<List<DeparmentResponse>>();

        }

        public async Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest Request,
     CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var entity = new Department
            {
                Name = Request.Name,
                FaculityId = Request.faculityId
            ,
            };
            await _context.Departments.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateDepartmentResponse>();
        }

        public async Task<UpdateDepartmentResponse> UpdateDepartment(UpdateDepartmentRequest Request,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var department = await _context.Departments.FindAsync(Request.Id) ?? throw new NotFoundException($"Department doesn't exist with id: {Request.Id}");
            department.Name = Request.Name;
            department.FaculityId = Request.faculityId;
            await _context.SaveChangesAsync(ct);
            return department.Adapt<UpdateDepartmentResponse>();
        }

        public async Task DeleteDepartment(int id, CancellationToken ct)
        {
            var result = await _context.Departments
                               .Where(x => x.Id == id).FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"department doesn't exist with id: {id}");
            }
            _context.Departments.Remove(result);
            await _context.SaveChangesAsync(ct);

        }
        //    public async Task<PaginationResponse<DeparmentResponse>> GetDepartmentByFilter(LazyLoadEvent request, CancellationToken ct)
        //    {
        //        var query = _context.Departments.Include(d => d.Faculity) // Ensure Faculty data is loaded
        //    .Where(x => x.IsDeleted != true) // Filter out deleted departments
        //    .AsQueryable();
        //        var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
        //                                        .LazyFilters(request)
        //                                        .LazyOrderBy(request)
        //                                        .LazySkipTake(request)
        //                                        .ToListAsync(ct)
        //                                        .ConfigureAwait(false)
        //        :
        //        await query.AsNoTracking()
        //            .LazySearch(request.GlobalFilter, "Name")
        //            .ToListAsync(ct)
        //            .ConfigureAwait(false);
        //        return new PaginationResponse<DeparmentResponse>
        //        {
        //            Data = result.Adapt<List<DeparmentResponse>>(),
        //            Total = await query.CountAsync(ct),
        //        };
        //    }
        //}

        public async Task<PaginationResponse<DeparmentResponse>> GetDepartmentByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.Departments
                .Include(d => d.Faculity) // Ensure Faculty data is loaded
                .Where(x => x.IsDeleted != true) // Filter out deleted departments
                .AsQueryable();

            var result = string.IsNullOrWhiteSpace(request.GlobalFilter)
                ? await query.AsNoTracking()
                    .LazyFilters(request)
                    .LazyOrderBy(request)
                    .LazySkipTake(request)
                    .Select(d => new DeparmentResponse(
                        d.Id,
                        d.Name,
                        d.FaculityId,
                        d.Faculity.Name // Get Faculty name
                    ))
                    .ToListAsync(ct)
                    .ConfigureAwait(false)
                : await query.AsNoTracking()
                    .LazySearch(request.GlobalFilter, "Name")
                    .Select(d => new DeparmentResponse(
                        d.Id,
                        d.Name,
                        d.FaculityId,
                        d.Faculity.Name
                    ))
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

            return new PaginationResponse<DeparmentResponse>
            {
                Data = result,
                Total = await query.CountAsync(ct)
            };
        }

    }
}
