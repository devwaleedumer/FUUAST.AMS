using AMS.MODELS.ApplicationForm.Applicant;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
    
}
