using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.Faculity;
using AMS.SERVICES.IDataService;
using Mapster;
using Microsoft.EntityFrameworkCore;
using AMS.MODELS.Filters;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;

namespace AMS.SERVICES.DataService
{
    public class FacultyService(AMSContext context) : IFacultyService
    {
        private readonly AMSContext _context = context;
        public async Task<List<FaculityResponse>> GetAllFaculties(CancellationToken ct)
        {
            var result = await _context.Faculties
                                                   .AsNoTracking()
                                                   .ToListAsync(ct)
                                                   .ConfigureAwait(false);
            return result.Adapt<List<FaculityResponse>>();
        }

        public async Task<PaginationResponse<FaculityResponse>> GetFacultiesByFilter(LazyLoadEvent request,CancellationToken ct)
        {
            var query =  _context.Faculties.AsQueryable();
                var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
                                                .LazyFilters(request)
                                                .LazyOrderBy(request)
                                                .LazySkipTake(request)  
                                                .ToListAsync(ct)
                                                .ConfigureAwait(false)
                :
                await query.AsNoTracking()
                    .LazySearch(request.GlobalFilter,"Name")
                    .ToListAsync(ct)
                    .ConfigureAwait(false);
                return new PaginationResponse<FaculityResponse>
                {
                    Data = result.Adapt<List<FaculityResponse>>(),
                    Total = await query.CountAsync(ct),
                };
        }
        public async Task<CreateFacultyResponse> CreateFaculty(CreateFacultyRequest facultyRequest,
            CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(facultyRequest);
            var entity = new Faculity { Name = facultyRequest.Name };
            await _context.Faculties.AddAsync(entity,ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateFacultyResponse>();
        }
        public async Task<UpdateFacultyResponse> UpdateFaculty(UpdateFacultyRequest facultyRequest,
            CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(facultyRequest);
             var faculty =await _context.Faculties.FindAsync(facultyRequest.Id) ?? throw new NotFoundException($"Faculty doesn't exist with id: {facultyRequest.Id}");
             faculty.Name = facultyRequest.Name;
             await _context.SaveChangesAsync(ct);
             return faculty.Adapt<UpdateFacultyResponse>();
        }
        public async Task DeleteFaculty( int facultyId, CancellationToken ct)
        {
            var faculty =await _context.Faculties.FindAsync(facultyId) ?? throw new NotFoundException($"Faculty doesn't exist with id: {facultyId}");
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync(ct);
        }
    }
}
