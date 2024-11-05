using AMS.MODELS.Academicyear;
using AMS.MODELS.Filters;
using AMS.MODELS.Session;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController(ISessionService service) : BaseApiController
    {
        private readonly ISessionService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllSession(CancellationToken ct)
        {
            return Ok(await _service.GetAllSession(ct));
        }
        [HttpDelete("DeleteSession")]

        public async Task<IActionResult> DeleteSession(int id, CancellationToken ct)
        {
            await _service.DeleteSession(id, ct);
            return NoContent();
        }

        [HttpPost("AddSession")]
        public async Task<IActionResult> CreateSession(CreateSessionRequest request, CancellationToken ct)
        {
            return Ok(await _service.CreateSession(request, ct));
        }
        [HttpPut("UpdateSession")]
        public async Task<IActionResult> UpdateSession(UpdateSessionRequest request, CancellationToken ct)
        {
            return Ok(await _service.UpdateSession(request, ct));
        }
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllSessionByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
              => Ok(await _service.GetSessionByFilter(request, ct));
    }

}

