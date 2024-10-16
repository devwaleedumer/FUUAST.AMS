using AMS.DATA;
using AMS.MODELS.Program;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace AMS.SERVICES.DataService
{
     public class ProgramService(AMSContext context): IProgramService
    {
        private readonly AMSContext _context = context;

       public async Task<List<ProgramResponse>> GetAllPrograms(CancellationToken ct)
        {
           var result = await _context.Programs
                                      .AsNoTracking()
                                      .ToListAsync(ct)
                                      .ConfigureAwait(false);
            return result.Adapt<List<ProgramResponse>>();
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


    }
}
