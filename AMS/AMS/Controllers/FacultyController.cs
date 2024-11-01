using AMS.MODELS.Faculity;
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
      [HttpPost]
        public async Task<IActionResult> CreateFaculty(CreateFacultyRequest request,CancellationToken ct)
            => CreatedAtAction(nameof(CreateFaculty),await _faculityService.CreateFaculty(request,ct));

        [HttpPut("{facultyId}")]
        public async Task<IActionResult> UpdateFaculty(UpdateFacultyRequest request, int facultyId, CancellationToken ct)
           =>  facultyId != request.Id
                ? BadRequest(new ProblemDetails(){Detail = "invalid Request, try again later"}) :
             Ok(await _faculityService.UpdateFaculty(request,ct));

        [HttpDelete("{facultyId}")]
        public async Task<IActionResult> DeleteFaculty(int facultyId, CancellationToken ct)
        {
            await _faculityService.DeleteFaculty(facultyId, ct);
             return NoContent();
        }
        
    }
}
