using AMS.MODELS.ApplicationForm;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AMS.Controllers
{
    [Authorize]
    public class ApplicationFormsController (IApplicationFormService applicationFormService) : BaseApiController
    {
        private readonly IApplicationFormService _applicationFormService = applicationFormService;
        [HttpPost]
        public async Task<IActionResult> CreateApplication(CreateApplicationFormRequest request, CancellationToken ct)
                => CreatedAtAction(nameof(CreateApplication), await _applicationFormService.CreateApplicationForm(request, ct));
        [HttpPut]
        public async Task<IActionResult> SubmitApplicationForm(SubmitApplicationFormRequest request, CancellationToken ct)
                => Ok(await _applicationFormService.SubmitApplicationForm(request, ct));
        [HttpGet("submitted-application")]
        public async Task<IActionResult> GetSubmitApplicationForm(CancellationToken ct)
                => Ok(await _applicationFormService.GetSubmittedApplication(ct));
        [HttpPut("submitted-application/{applicationFormId}")]
        public async Task<IActionResult> EditSubmitApplicationForm(int applicationFormId, EditSubmitApplicationFormRequest request, CancellationToken ct)
        {
            if (request.Id != applicationFormId)
                return BadRequest("Invalid request");
            return Ok(await _applicationFormService.EditSubmittedApplication(request,ct));
        }
        [HttpGet("{userId}/applicant-dashboard-status")]
        public async Task<IActionResult> GetSubmitApplicationForm(int userId,CancellationToken ct)
        {
            if (HttpContext.User.GetUserId() != userId)
                return BadRequest("Invalid request");
            return Ok(await _applicationFormService.GetApplicationFormStatus(userId, ct));
        }
    }
}
