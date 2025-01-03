using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.ProgramType;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace AMS.SERVICES.DataService
{
    public class ProgramTypeService(AMSContext context) : IProgramtypeService
    {
        private readonly AMSContext _context = context;

        public async Task<List<ProgramtypeResponse>> GetAllProgramType(CancellationToken ct)
        {
            var result = await _context.ProgramTypes
                                       .AsNoTracking()
                                       .Where(x=>x.IsDeleted !=true)
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<ProgramtypeResponse>>();

        }


        public async Task<CreateProgramTypeResponse> CreateProgramType(CreateProgramTypeRequest Request,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var entity = new ProgramType
            {
                Name = Request.Name,

            };
            await _context.ProgramTypes.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateProgramTypeResponse>();
        }

        public async Task<UpdateProgramTypeResponse> UpdateProgramType(UpdateProgramTypeRequest request,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            var type = await _context.ProgramTypes.FindAsync(request.Id) ?? throw new NotFoundException($"program type doesn't exist with id: {request.Id}");
            type.Name = request.Name;
            await _context.SaveChangesAsync(ct);
            return type.Adapt<UpdateProgramTypeResponse>();
        }

        public async Task DeleteProgramType(int id, CancellationToken ct)
        {
            var result = await _context.ProgramTypes
                                .FirstOrDefaultAsync(x => x.Id == id,ct)
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"Faculty doesn't exist with id: {id}");
            }
            _context.ProgramTypes.Remove(result);
            await _context.SaveChangesAsync(ct);

        }
        public async Task<PaginationResponse<ProgramtypeResponse>> GetProgramtypeByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.ProgramTypes.AsQueryable();
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
            return new PaginationResponse<ProgramtypeResponse>
            {
                Data = result.Adapt<List<ProgramtypeResponse>>(),
                Total = await query.CountAsync(ct),
            };
        }

    }
}