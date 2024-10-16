using AMS.DATA;
using AMS.MODELS.Faculity;
using AMS.SERVICES.IDataService;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
