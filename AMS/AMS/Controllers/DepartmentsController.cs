using AMS.SERVICES.DataService;
using AMS.SERVICES.IDataService;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class DepartmentsController(IDepartmentService departmentService, IShiftService shiftService) : BaseApiController
    {
        private readonly IDepartmentService _departmentService = departmentService;
        private readonly IShiftService _shiftService = shiftService;
        [HttpGet("{departmentId:int}/{programId:int}/shifts")]
        public async Task<IActionResult> GetTimeShiftByDepartmentId(int departmentId,int programId, CancellationToken ct)
                => Ok(await _shiftService.GetTimeShiftByDepartmentAndProgramId(departmentId, programId,ct));
    }
}
