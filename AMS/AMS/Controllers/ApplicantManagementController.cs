using AMS.MODELS;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.Filters;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace AMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicantManagementController(IApplicantManagementService service) : BaseApiController
    {
        private readonly IApplicantManagementService _service = service;


        [HttpPost]
        public async Task<IActionResult> GetAllApplicantDetails(ApplicantInfoRequest applicantInfoRequest)
        {
            return Ok(await _service.GetAllApplicantDetails(applicantInfoRequest));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateApplicantDetails(updateApplicantRequest request)
        {
            return Ok(await _service.UpdateApplicantDetails(request));
        }

        [HttpGet("get-merit-list-report/{id:int:required}")]
        public async Task<IActionResult> GetChallan(int id, CancellationToken ct)
        {
            var data = await _service.GetMeritListData(id);
            var pdf = new ViewAsPdf(@"./Reports/merit-list.cshtml", data);
            var bytes = await pdf.BuildFile(ControllerContext);
            return File(bytes, "application/pdf", "merit-list.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> GetAllMeritListsByFilter(LazyLoadEvent request,CancellationToken cancellationToken)
        {
            return Ok(await _service.GetAllMeritListDetailsByFilter(request, cancellationToken));
        }
        
        [HttpPost]
        public async Task<IActionResult> GenerateMeritList(GenerateMeritListRequest request,CancellationToken cancellationToken)
        {
            await _service.GenerateMeritList(request);
            return Ok();
        }
    }
    
}
