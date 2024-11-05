using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Shift;
using AMS.SERVICES.DataService;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class ShiftController(IShiftService service) : BaseApiController
    {
        private readonly IShiftService _service = service;

        [HttpGet("GetAllShift")]
        public async Task<IActionResult> GetAllShift(CancellationToken ct)
        {
            return Ok(await _service.GetAllShift(ct));
        }

        [HttpDelete("DeleteShift")]
       
        public async Task<IActionResult> DeleteShift(int id,CancellationToken ct)
        {
            await _service.DeleteShift(id, ct);
            return NoContent();
        }

        [HttpPost("AddShift")]
        public async Task<IActionResult> CreateShift(CreateShiftRequest request, CancellationToken ct)
        {
            return Ok(await _service.CreateFaculty(request,ct));
        }
        [HttpPut("UpdateShift")]
        public async Task<IActionResult> updateShift(UpdateShiftRequest request, CancellationToken ct)
        {
           return Ok(await _service.UpdateShift(request, ct));
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetAllShiftByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
                => Ok(await _service.GetShiftByFilter(request, ct));

    }
}
