
using AMS.MODELS.Filters;
using AMS.MODELS.ProgramType;
using AMS.MODELS.Shift;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramtypeController(IProgramtypeService service) : ControllerBase
    {
        private readonly IProgramtypeService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllProgramtype(CancellationToken ct)
        {
            return Ok(await _service.GetAllProgramType(ct));
        }


        [HttpDelete("DeleteProgramtype")]

        public async Task<IActionResult> DeleteProgramtype(int id, CancellationToken ct)
        {
            await _service.DeleteProgramType(id, ct);
            return NoContent();
        }

        [HttpPost("AddProgramtype")]
        public async Task<IActionResult> CreateProgramtype(CreateProgramTypeRequest request, CancellationToken ct)
        {
            return Ok(await _service.CreateProgramType(request, ct));
        }
        [HttpPut("UpdateProgramtype")]
        public async Task<IActionResult> UpdateProgramType(UpdateProgramTypeRequest request, CancellationToken ct)
        {
            return Ok(await _service.UpdateProgramType(request, ct));
        }
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllProgramtypeByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
              => Ok(await _service.GetProgramtypeByFilter(request, ct));
    }
}
