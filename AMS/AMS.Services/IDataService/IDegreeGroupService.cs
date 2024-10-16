using AMS.MODELS.DegreeGroup;

namespace AMS.SERVICES.IDataService
{
    public interface IDegreeGroupService
    {
         Task<Dictionary<string, List<DegreeGroupResponse>>> GetAllDegreeGroupWithDegreeLevels(CancellationToken ct);

    }
}