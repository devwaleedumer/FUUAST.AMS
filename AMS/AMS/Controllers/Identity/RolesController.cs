using AMS.MODELS.Identity.Roles;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AMS.Controllers.Identity;

public class RolesController(IRoleService roleService) : BaseApiController
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> GetAllRoles(CancellationToken ct) => Ok(await _roleService.GetAllRolesAsync(ct));
   
    [HttpGet("roles-with-permissions")]
    public async Task<IActionResult> GetAllRolesWithPermissions(CancellationToken ct) => Ok(await _roleService.GetAllRolesWithPermissionsAsync(ct));

    [HttpGet("all-permissions")]
    public async Task<IActionResult> GetAllPermissions(CancellationToken ct) => Ok(await _roleService.GetAllPermissionsAsync(ct));

    [HttpPut("{id}/permissions")]
    public async Task<IActionResult> UpdatePermissions(UpdateRolePermissionsRequest request, int id, CancellationToken ct) => request.Id !=  id ? BadRequest(new
    {
        Message = "Invalid Request"
    }) : Ok(new
    {
        Message = await _roleService.UpdatePermissionsAsync(request, ct)
    });

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole([FromRoute] int id, [FromBody] CreateOrUpdateRoleRequest request, CancellationToken ct) 
    {
        if (id == request.Id)
        {
          return  Ok(new
            {
                Message = await _roleService.CreateOrUpdateAsync(request)
            });
        }
        return BadRequest(new {
            Message = "Invalid Request"
        });
    } 

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateOrUpdateRoleRequest request, CancellationToken ct) => Ok(new
    {
        Message = await _roleService.CreateOrUpdateAsync(request)
    });
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole([FromRoute]int id) => Ok(new
    {
        Message = await _roleService.DeleteAsync(id.ToString())
    });

}