using AMS.MODELS.FeeChallan;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace AMS.Controllers
{
    public class FeeChallansController (IFeeChallanService feeChallan) : BaseApiController
    {
        private readonly IFeeChallanService _challanService = feeChallan;
        [HttpGet("get-challan/{id:int:required}")]
        public async Task<IActionResult> GetChallan(int id,CancellationToken ct)
        {
            var data = await _challanService.GetFeeChallanData(id, ct);
            var pdf =  new ViewAsPdf(@"./Reports/fee/FeeChallanUg.cshtml",data);
            var bytes = await pdf.BuildFile(ControllerContext);
            return File(bytes, "application/pdf", "fee_challan.pdf");
        }

        [HttpGet("exists/{applicantId:int:required}")]
        public async Task<IActionResult> CheckChallanByApplicant(int applicantId, CancellationToken cancellation)
            => Ok(await _challanService.FeeChallanExists(applicantId, cancellation));
       
        [HttpPost("upload-challan/{feeChallanId:int:required}")]
        public async Task<IActionResult> UploadFeeChallan(int feeChallanId,FeeChallanSubmissionRequest request,CancellationToken ct)
        {
            await _challanService.UploadFeeChallanImage(feeChallanId,request, ct);
            return Ok();
        }

    }
}
