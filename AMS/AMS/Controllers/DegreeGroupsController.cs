using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    
    public class DegreeGroupsController(IDegreeGroupService degreeGroup) : BaseApiController
    {
        private readonly IDegreeGroupService _degreeGroupService =degreeGroup;
        [HttpGet("degree-levels")]
        public async Task<IActionResult> GetAllDegreeGroupsByDict(CancellationToken ct) => Ok(await _degreeGroupService.GetAllDegreeGroupWithDegreeLevels(ct));
    }
}
