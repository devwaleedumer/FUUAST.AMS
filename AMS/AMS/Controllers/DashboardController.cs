using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
   
    public class DashboardController:BaseApiController
    {
        private IDashboardservice _service;
        public DashboardController(IDashboardservice service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task <IActionResult> Dashboard()
        {
            var dashboard= await _service.Dashboard();
            return Ok(dashboard);
        }       
    }
}
