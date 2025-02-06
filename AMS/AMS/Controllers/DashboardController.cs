using AMS.MODELS.Filters;
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

        [HttpGet("fee-challan-revenue")]
        public async Task<IActionResult> FeeChallanRevenue()
        {
            var data = await _service.FeeRevenueData();
            return Ok(data);
        }

        [HttpGet("application-by-departments")]
        public async Task<IActionResult> ApplicationByDepartments()
        {
            var data = await _service.ApplicationsByDepartmentData();
            return Ok(data);
        }
        [HttpPost("audit-data")]
        public async Task<IActionResult> ApplicationByDepartments(LazyLoadEvent request, CancellationToken cancellation)
        {
            var data = await _service.AuditData(request,cancellation);
            return Ok(data);
        }
    }
}
