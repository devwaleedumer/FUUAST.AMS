using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Http;
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
    }
}
