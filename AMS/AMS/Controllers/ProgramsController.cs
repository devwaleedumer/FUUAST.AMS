using AMS.MODELS.Filters;
using AMS.MODELS.Program;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class ProgramsController(IProgramService service) : BaseApiController
    {
       private readonly IProgramService _service = service;
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
