using AMS.MODELS.Filters;
using AMS.SERVICES.DataService;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class FacultyController(IFacultyService faculityService, IDepartmentService departmentService) : BaseApiController
    {
        private readonly IFacultyService _faculityService = faculityService;
        private readonly IDepartmentService _departmentService = departmentService;

        [HttpGet]
        public async Task<IActionResult> GetAllFaculties(CancellationToken ct)
                 => Ok(await _faculityService.GetAllFaculties(ct));
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllFacultiesByFilter([FromBody]LazyLoadEvent request,CancellationToken ct)
                 => Ok(await _faculityService.GetFacultiesByFilter(request,ct));

        [HttpGet("{facultyId}/{programId}/departments")]
        public async Task<IActionResult> GetDepartmentsByFacultyId(int facultyId,int programId, CancellationToken ct)
                => Ok(await _departmentService.GetDepartmentsByFacultyId(facultyId, programId, ct));
    }
}
