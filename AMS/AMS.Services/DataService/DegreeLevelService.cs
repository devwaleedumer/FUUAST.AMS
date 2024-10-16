using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.DegreeGroup;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.DataService
{
    public class DegreeLevelService(AMSContext context)
    {
        private readonly AMSContext _context = context;
        public Task<Dictionary<string, List<DegreeGroupResponse>>> GetAllDegreeGroupWithDegreeLevels(CancellationToken ct)
            => _context.DegreeLevels
                         .AsNoTracking()
                         .Include(x => x.DegreeGroups)
                         .ToDictionaryAsync(
                            key => key.Name,
                            value => (value.DegreeGroups ?? Enumerable.Empty<DegreeGroup>())
                                .Adapt<List<DegreeGroupResponse>>());

    }
}
