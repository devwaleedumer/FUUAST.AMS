using AMS.MODELS.ApplicationForms.Ug;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ApplicationFormController : ControllerBase
    {
        private readonly IApplicationFormService _service;
       public ApplicationFormController(IApplicationFormService service) {
            _service = service;
        }


        [HttpPost]
        public async Task <IActionResult> AddApplicationForm([FromBody]ApplicationRequest request)
        {
            return Ok(await _service.AddApplicationForm(request));
        }
    }
}
