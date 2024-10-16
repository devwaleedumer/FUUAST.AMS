using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.DegreeGroup;
using AMS.SERVICES.IDataService;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace AMS.SERVICES.DataService
{
    public class DegreeGroupService(AMSContext context) : IDegreeGroupService
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
