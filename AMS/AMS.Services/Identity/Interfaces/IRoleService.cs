using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.MODELS.Identity.Roles;

namespace AMS.SERVICES.Identity.Interfaces
{
    public interface IRoleService
    {
        //Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task<List<RoleDto>> GetAllRolesAsync(CancellationToken ct);

        //Task<RoleDto> GetByIdAsync(int id);

        Task<RolePermissionDto> GetByIdWithPermissionsAsync(int roleId, CancellationToken cancellationToken);

        //Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

        Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

        //Task<string> DeleteAsync(int id);
        Task<List<string>> GetAllPermissionsAsync(CancellationToken ct);

        Task<bool> ExistsAsync(string roleName, int? excludeId);

        Task<List<RolePermissionDto>> GetAllRolesWithPermissionsAsync(CancellationToken ct);

        Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

        Task<string> DeleteAsync(string id);

    }
}
