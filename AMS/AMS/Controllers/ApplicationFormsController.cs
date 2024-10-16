using AMS.MODELS.ApplicationForm;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class ApplicationFormsController (IApplicationFormService applicationFormService) : BaseApiController
    {
        private readonly IApplicationFormService _applicationFormService = applicationFormService;
        [HttpPost]
        public async Task<IActionResult> CreateApplication(CreateApplicationFormRequest request, CancellationToken ct)
                => CreatedAtAction(nameof(CreateApplication), await _applicationFormService.CreateApplicationForm(request, ct));
        [HttpPut]
        public async Task<IActionResult> SubmitApplicationForm(SubmitApplicationFormRequest request, CancellationToken ct)
                => Ok(await _applicationFormService.SubmitApplicationForm(request, ct));

    }
}
