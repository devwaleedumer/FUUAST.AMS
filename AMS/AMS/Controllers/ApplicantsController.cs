using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.ApplicationForm.ApplicantDegree;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Authorize]
    [ApiController]
    public class ApplicantsController(IApplicantService applicantService) : BaseApiController
    {
        private readonly IApplicantService _applicantService = applicantService;
       
        [HttpPost("personal-information")]
        public async Task<IActionResult> CreatePersonalInformation([FromBody] CreateApplicantPSInfoRequest request, CancellationToken cancellationToken)
        {
            var result = await _applicantService.AddApplicantPersonalInformation(request, cancellationToken);
            return CreatedAtAction(nameof(CreatePersonalInformation), new { Id = result.Id }, result);
        }

        [HttpGet("personal-information")]
        public async Task<IActionResult> GetPersonalInformation(CancellationToken cancellationToken) => Ok(await _applicantService.GetApplicantPersonalInformation(cancellationToken));
     
        [HttpPut("personal-information/{applicantId}")]
        public async Task<IActionResult> UpdatePersonalInformation(int applicantId,[FromBody] UpdateApplicantPSInfoRequest request,CancellationToken cancellationToken) => 
           applicantId == request.Id ?  Ok(await _applicantService.UpdateApplicantPersonalInformation(request, cancellationToken)) : BadRequest();

        [HttpPost("degrees")]
        public async Task<IActionResult> CreateApplicantDegrees([FromBody]CreateApplicantDegreeListRequest request, CancellationToken cancellationToken) 
        { 
           var result = await _applicantService.AddApplicantDegrees(request, cancellationToken);
            return CreatedAtAction(nameof(CreatePersonalInformation), result);
        }

        [HttpGet("degrees")]
        public async Task<IActionResult> GetApplicantDegrees(CancellationToken ct) => Ok(await _applicantService.GetApplicantDegrees(ct));

        [HttpPut("degrees")]
        public async Task<IActionResult> EditApplicantDegrees(EditApplicantDegreeListRequest request, CancellationToken ct) => Ok(await _applicantService.EditApplicantDegrees(request,ct));
       
    }
}
