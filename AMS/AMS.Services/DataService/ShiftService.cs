using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Program;
using AMS.MODELS.Shift;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AMS.SERVICES.DataService
{
    public class ShiftService(AMSContext context) : IShiftService
    {
        private readonly AMSContext _context = context;
        public async Task<List<ShiftResponse>> GetTimeShiftByDepartmentAndProgramId(int departmentId, int programId, CancellationToken ct)
        {
            var result = await _context.ProgramDepartments
                                       .AsNoTracking()
                                       .Where(pd => pd.DepartmentId == departmentId && pd.ProgramId == programId)
                                       .Select(x => x.TimeShift)
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<ShiftResponse>>();
        }

        public async Task <List<ShiftResponse>> GetAllShift(CancellationToken ct)
        {
            var result = await _context.TimeShifts
                                .AsNoTracking().Where(x=>x.IsDeleted==false)
                                .ToListAsync()
                                .ConfigureAwait(false);
            return result.Adapt<List<ShiftResponse>>();
        }

        public async Task<CreateShiftResponse> CreateFaculty(CreateShiftRequest shiftRequest,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(shiftRequest);
            var entity = new TimeShift 
            { 
                Name = shiftRequest.Name,
                Description=shiftRequest.Description,
            };
            await _context.TimeShifts.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateShiftResponse>();
        }

        public async Task<UpdateShiftResponse> UpdateShift(UpdateShiftRequest shiftRequest,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(shiftRequest);
            var shift = await _context.TimeShifts.FindAsync(shiftRequest.Id) ?? throw new NotFoundException($"shift doesn't exist with id: {shiftRequest.Id}");
            shift.Name = shiftRequest.Name;
            await _context.SaveChangesAsync(ct);
            return shift.Adapt<UpdateShiftResponse>();
        }

        public async Task DeleteShift(int id,CancellationToken ct)
        {
            var result = await _context.TimeShifts
                               .Where(x => x.Id ==id ).FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"Faculty doesn't exist with id: {id}");
            }
            _context.TimeShifts.Remove(result);
            await _context.SaveChangesAsync(ct);
           
        }

        public async Task<PaginationResponse<ShiftResponse>> GetShiftByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.TimeShifts.AsQueryable();
            var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
                                            .LazyFilters(request)
                                            .LazyOrderBy(request)
                                            .LazySkipTake(request)
                                            .ToListAsync(ct)
                                            .ConfigureAwait(false)
            :
            await query.AsNoTracking()
                .LazySearch(request.GlobalFilter, "Name")
                .ToListAsync(ct)
                .ConfigureAwait(false);
            return new PaginationResponse<ShiftResponse>
            {
                Data = result.Adapt<List<ShiftResponse>>(),
                Total = await query.CountAsync(ct),
            };
        }
    }
    }
