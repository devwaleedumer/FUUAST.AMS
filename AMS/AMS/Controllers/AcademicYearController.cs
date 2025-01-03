using AMS.Authorization.Permissons;
using AMS.MODELS.Academicyear;
using AMS.MODELS.Filters;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Constants.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
   [Authorize]
    public class AcademicYearController(IAcademicyearService service) : BaseApiController
    {
        private readonly IAcademicyearService _service = service;

        [MustHavePermission(AMSAction.View,AMSResource.Dashboard)]
        [HttpGet("GetAllAcademicyear")]
        public async Task<IActionResult> GetAllAcedamicYear(CancellationToken ct)
        {
            return Ok(await _service.GetAllAcedamicYear(ct));
        }

        [HttpDelete("DeleteAcademicyear")]

        public async Task<IActionResult> DeleteAcademicYear(int id, CancellationToken ct)
        {
            await _service.DeleteAcademicYear(id, ct);
            return NoContent();
        }

        [HttpPost("AddAcademicyear")]
        public async Task<IActionResult> CreateAcademicyear(CreateAcademicyearRequest request, CancellationToken ct)
        {
            return Ok(await _service.CreateAcademicyear(request, ct));
        }
        [HttpPut("UpdateAcademicyear")]
        public async Task<IActionResult> UpdateAcademicyear(UpdateAcademicyearRequest request, CancellationToken ct)
        {
            return Ok(await _service.UpdateAcademicyear(request, ct));
        }
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllAcademicYearByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
             => Ok(await _service.GetAcademicYearByFilter(request, ct));
    }
}

