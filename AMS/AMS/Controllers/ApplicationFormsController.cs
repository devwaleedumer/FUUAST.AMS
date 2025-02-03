using AMS.MODELS.ApplicationForm;
using AMS.SERVICES.IDataService;
using AMS.SERVICES.Reporting.IService;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Security.Claims;

namespace AMS.Controllers
{
    public class ApplicationFormsController (IApplicationFormService applicationFormService,IApplicationFormReportService formReportService) : BaseApiController
    {
        private readonly IApplicationFormService _applicationFormService = applicationFormService;
        private readonly IApplicationFormReportService _formReportService = formReportService;
        [HttpPost]
        public async Task<IActionResult> CreateApplication(CreateApplicationFormRequest request, CancellationToken ct)
                => CreatedAtAction(nameof(CreateApplication), await _applicationFormService.CreateApplicationForm(request, ct));
        [HttpPut]
        public async Task<IActionResult> SubmitApplicationForm(SubmitApplicationFormRequest request, CancellationToken ct)
                => Ok(await _applicationFormService.AddApplicationFormPrograms(request, ct));
        [HttpGet("application-programs")]
        public async Task<IActionResult> GetSubmitApplicationForm(CancellationToken ct)
                => Ok(await _applicationFormService.GetApplicationFormPrograms(ct));
        [HttpPut("application-programs/{applicationFormId}")]
        public async Task<IActionResult> EditSubmitApplicationForm(int applicationFormId, EditSubmitApplicationFormRequest request, CancellationToken ct)
        {
            if (request.Id != applicationFormId)
                return BadRequest("Invalid request");
            return Ok(await _applicationFormService.EditApplicationFormPrograms(request,ct));
        }
        [HttpGet("{userId}/applicant-dashboard-status")]
        public async Task<IActionResult> GetSubmitApplicationForm(int userId,CancellationToken ct)
        {
            if (HttpContext.User.GetUserId() != userId)
                return BadRequest("Invalid request");
            return Ok(await _applicationFormService.GetApplicationFormStatus(userId, ct));
        }

        [HttpGet("get-details/{id:int:required}")]
        public async Task<IActionResult> GetApplicationDetails(int id, CancellationToken ct)
        {
            return Ok(await _applicationFormService.ApplicationDetailsByApplicantId(id,ct));
        }
        [HttpGet("report/{id}")]
        public async Task<IActionResult> ApplicationFormReport(int id, CancellationToken ct)
        {
            return File(await _formReportService.GenerateUGApplicationFormPdfReportAsync(id), "application/pdf","admission_form.pdf");
        }
    }
}
