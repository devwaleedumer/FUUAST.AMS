using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
   // [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly IUploadImageService _service;


            public UploadImageController(IUploadImageService service)
        {
            _service = service;
        }
        [HttpPost]
       public async Task<IActionResult> UploadProfilePicture(IFormFile picture)
        {
            var response = await _service.UploadProfilePicture(picture);
            return Ok(response);
        }
    }
}
