using AMS.MODELS.Department;
using AMS.MODELS.Filters;
using AMS.MODELS.Program;
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
        public async Task<IActionResult> GetTimeShiftByDepartmentId(int departmentId, int programId, CancellationToken ct)
                => Ok(await _shiftService.GetTimeShiftByDepartmentAndProgramId(departmentId, programId, ct));

        [HttpGet]
        public async Task<IActionResult> GetAllDepartment(CancellationToken ct)
        {
            return Ok(await _departmentService.GetAllDepartment(ct));
        }
        [HttpDelete("DeleteDepartment")]

        public async Task<IActionResult> DeleteDepartment(int id, CancellationToken ct)
        {
            await _departmentService.DeleteDepartment(id, ct);
            return NoContent();
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentRequest request, CancellationToken ct)
        {
            return Ok(await _departmentService.CreateDepartment(request, ct));
        }
        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentRequest request, CancellationToken ct)
        {
            return Ok(await _departmentService.UpdateDepartment(request, ct));
        }
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllDepartmentByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
              => Ok(await _departmentService.GetDepartmentByFilter(request, ct));
    }
}