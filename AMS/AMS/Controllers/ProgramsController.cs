using AMS.MODELS.Filters;
using AMS.MODELS.Program;
using AMS.SERVICES.DataService;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class ProgramsController(IProgramService service, IDepartmentService departmentService) : BaseApiController
    {
       private readonly IProgramService _service = service;
       private readonly IDepartmentService _departmentService = departmentService;



        [HttpGet("{programId:int}/departments")]
        public async Task<IActionResult> GetTimeShiftByDepartmentId( int programId, CancellationToken ct)
              => Ok(await _departmentService.GetDepartmentsByProgramId(programId, ct));

        [HttpGet]
        public async Task<IActionResult> GetAllPrograms(CancellationToken ct)
        {
            return Ok(await _service.GetAllPrograms(ct));
        }
        [HttpDelete("DeleteProgram")]

        public async Task<IActionResult> DeleteProgram(int id, CancellationToken ct)
        {
            await _service.DeleteProgram(id, ct);
            return NoContent();
        }

        [HttpPost("AddProgram")]
        public async Task<IActionResult> CreateProgram(CreateProgramRequest request, CancellationToken ct)
        {
            return Ok(await _service.CreateProgram(request, ct));
        }
        [HttpPut("UpdateProgram")]
        public async Task<IActionResult> UpdateProgram(UpdateProgramRequest request, CancellationToken ct)
        {
            return Ok(await _service.UpdateProgram(request, ct));
        }
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllProgramByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
              => Ok(await _service.GetProgramByFilter(request, ct));
    }
}
