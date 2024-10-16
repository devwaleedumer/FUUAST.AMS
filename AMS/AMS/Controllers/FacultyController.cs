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
        public async Task<IActionResult> GetAllFaculities(CancellationToken ct)
                 => Ok(await _faculityService.GetAllFaculties(ct));

        [HttpGet("{faculityId}/departments")]
        public async Task<IActionResult> GetDepartmentsByFacultyId(int faculityId, CancellationToken ct)
                => Ok(await _departmentService.GetDepartmentsByFacultyId(faculityId, ct));
    }
}
