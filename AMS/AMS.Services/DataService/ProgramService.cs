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
     public class ProgramService(AMSContext context): IProgramService
    {
        private readonly AMSContext _context = context;

       public async Task<List<ProgramResponse>> GetAllPrograms(CancellationToken ct)
        {
            var programs = await _context.Programs
       .Include(p => p.ProgramType)
       .Where(x => x.IsDeleted != true)// Ensure ProgramType is loaded
       .Select(p => new
       {
           p.Id,
           p.Name,
           p.ProgramTypeId,
           ProgramType = p.ProgramType.Name // Assuming ProgramType has a Name field
       })
       .ToListAsync(ct)
       .ConfigureAwait(false);

            // Map the result to List<ProgramResponse>
            return programs.Adapt<List<ProgramResponse>>();
        
    }
        public async Task<ProgramResponse> GetProgramByApplicantId(int userId, CancellationToken ct)
        {
            var result = await _context.ApplicationForms
                          .Include(x => x.Applicant)
                          .AsNoTracking()
                          .Where(x => x.Applicant!.ApplicationUserId == userId)
                          .Select(y => y.Program)
                          .FirstOrDefaultAsync(ct)
                          .ConfigureAwait(false) ?? throw new NotFoundException("No program found for applicant");
            return result.Adapt<ProgramResponse>();
        }

        public async Task<CreateProgramResponse> CreateProgram(CreateProgramRequest Request,
        CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var entity = new Program
            {
                Name = Request.Name,
                ProgramTypeId = Request.ProgramTypeId
            ,
            };
            await _context.Programs.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateProgramResponse>();
        }

        public async Task<UpdateProgramResponse> UpdateProgram(UpdateProgramRequest Request,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var program = await _context.Programs.FindAsync(Request.Id) ?? throw new NotFoundException($"Program doesn't exist with id: {Request.Id}");
            program.Name = Request.Name;
            program.ProgramTypeId= Request.ProgramTypeId;
            await _context.SaveChangesAsync(ct);
            return program.Adapt<UpdateProgramResponse>();
        }

        public async Task DeleteProgram(int id, CancellationToken ct)
        {
            var result = await _context.Programs
                               .Where(x => x.Id == id).FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"program doesn't exist with id: {id}");
            }
            _context.Programs.Remove(result);
            await _context.SaveChangesAsync(ct);

        }
        //public async Task<PaginationResponse<ProgramResponse>> GetProgramByFilter(LazyLoadEvent request, CancellationToken ct)
        //{
        //    var query = _context.Programs.AsQueryable();
        //    var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
        //                                    .LazyFilters(request)
        //                                    .LazyOrderBy(request)
        //                                    .LazySkipTake(request)
        //                                    .ToListAsync(ct)
        //                                    .ConfigureAwait(false)
        //    :
        //    await query.AsNoTracking()
        //        .LazySearch(request.GlobalFilter, "Name")
        //        .ToListAsync(ct)
        //        .ConfigureAwait(false);
        //    return new PaginationResponse<ProgramResponse>
        //    {
        //        Data = result.Adapt<List<ProgramResponse>>(),
        //        Total = await query.CountAsync(ct),
        //    };
        //}
        public async Task<PaginationResponse<ProgramResponse>> GetProgramByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.Programs
                .Include(p => p.ProgramType) // Include ProgramType for accessing its properties
                .Where(x => x.IsDeleted != true)
                .AsQueryable();

            var result = string.IsNullOrWhiteSpace(request.GlobalFilter)
                ? await query.AsNoTracking()
                    .LazyFilters(request)
                    .LazyOrderBy(request)
                    .LazySkipTake(request)
                    .Select(p => new ProgramResponse(
                        p.Id,
                        p.Name,
                        p.ProgramTypeId,
                        p.ProgramType.Name // Get ProgramType name
                    ))
                    .ToListAsync(ct)
                    .ConfigureAwait(false)
                : await query.AsNoTracking()
                    .LazySearch(request.GlobalFilter, "Name")
                    .Select(p => new ProgramResponse(
                        p.Id,
                        p.Name,
                        p.ProgramTypeId,
                        p.ProgramType.Name
                    ))
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

            return new PaginationResponse<ProgramResponse>
            {
                Data = result,
                Total = await query.CountAsync(ct)
            };
        }

    }
}
