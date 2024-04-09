using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.Identity.Interfaces
{
    public interface IRoleService
    {
        //Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);

        //Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string roleName, int? excludeId);

        //Task<RoleDto> GetByIdAsync(int id);

        //Task<RoleDto> GetByIdWithPermissionsAsync(int roleId, CancellationToken cancellationToken);

        //Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

        //Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

        //Task<string> DeleteAsync(int id);
    }
}
