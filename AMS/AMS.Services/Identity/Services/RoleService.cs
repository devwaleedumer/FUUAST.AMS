using AMS.DOMAIN.Identity;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.MODELS.Identity.Roles;
using Mapster;
using Microsoft.EntityFrameworkCore;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Constants.Authorization;
using AMS.SHARED.Extensions;
using AMS.DATA;
using Microsoft.IdentityModel.Tokens;

namespace AMS.SERVICES.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AMSContext _context;

        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            AMSContext context
            )
        {
            _roleManager = roleManager;;
            _userManager = userManager; ;
            _context = context;
        }
        public async Task<bool> ExistsAsync(string roleName, int? excludeId) =>
        await _roleManager.FindByNameAsync(roleName)
         is ApplicationRole existingRole
         && existingRole.Id != excludeId;

        public async Task<List<RoleDto>> GetAllRolesAsync(CancellationToken ct) => 
            (await _roleManager.Roles.ToListAsync(ct)).Adapt<List<RoleDto>>();

        public async Task<List<RolePermissionDto>> GetAllRolesWithPermissionsAsync(CancellationToken ct)
           =>   await _roleManager.Roles
                .Include(rc => rc.RoleClaims)
                .Select(x => new RolePermissionDto
                 {
                Name = x.Name!,
                Description = x.Description ?? "",
                Id = x.Id,
                Permissions = x.RoleClaims.Select(x => x.ClaimValue!).ToList()

             }).ToListAsync();


        public async Task<List<string>> GetAllPermissionsAsync(CancellationToken ct) => 
            await _context.RoleClaims.Select(role => role.ClaimValue!).ToListAsync(ct);
        public async Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            _ = role ?? throw new NotFoundException("Role Not Found");
            if (role.Name == AMSRoles.SuperAdmin)
            {
                throw new ConflictException("Not allowed to modify Permissions for this Role.");
            }

            var currentClaims = await _roleManager.GetClaimsAsync(role);

            // Remove permissions that were previously selected
            foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                {
                    throw new AMSException("Update permissions failed.", removeResult.GetErrors());
                }
            }

            // Add all permissions that were not previously selected
            foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
            {
                if (!string.IsNullOrEmpty(permission))
                {
                    _context.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = AMSClaims.Permission,
                        ClaimValue = permission,
                    });
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }


            return "Permissions Updated.";
        }
        public async Task<RoleDto> GetByIdAsync(int id) =>
        await _context.Roles.SingleOrDefaultAsync(x => x.Id == id) is { } role
        ? role.Adapt<RoleDto>()
        : throw new NotFoundException("Role Not Found");

        public async Task<RolePermissionDto> GetByIdWithPermissionsAsync(int roleId, CancellationToken cancellationToken)
        {
            var role = await GetByIdAsync(roleId);
            var rolePermission = new RolePermissionDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
            rolePermission.Permissions = await _context.RoleClaims
                .Where(c => c.RoleId == roleId && c.ClaimType == AMSClaims.Permission)
                .Select(c => c.ClaimValue!)
                .ToListAsync(cancellationToken);

            return rolePermission;
        }

        public async Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request)
        {
            if (request.Id == null)
            {
                // Create a new role.
                var role = new ApplicationRole(request.Name, request.Description);
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new AMSException("Register role failed", result.GetErrors());
                }


                return string.Format("Role {0} Created.", request.Name);
            }
            else
            {
                // Update an existing role.
                var role = await _roleManager.FindByIdAsync(request.Id.ToString()!);

                _ = role ?? throw new NotFoundException("Role Not Found");

                if (AMSRoles.IsDefault(role.Name!))
                {
                    throw new ConflictException(string.Format("Not allowed to modify {0} Role.", role.Name));
                }

                role.Name = request.Name;
                role.NormalizedName = request.Name.ToUpperInvariant();
                role.Description = request.Description;
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    throw new AMSException("Update role failed", result.GetErrors());
                }

                return string.Format("Role {0} Updated.", role.Name);
            }
        }
        public async Task<string> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            _ = role ?? throw new NotFoundException("Role Not Found");

            if (AMSRoles.IsDefault(role.Name!))
            {
                throw new ConflictException(string.Format("Not allowed to delete {0} Role.", role.Name));
            }

            if ((await _userManager.GetUsersInRoleAsync(role.Name!)).Count > 0)
            {
                throw new ConflictException(string.Format("Not allowed to delete {0} Role as it is being used.", role.Name));
            }

            await _roleManager.DeleteAsync(role);


            return string.Format("Role {0} Deleted.", role.Name);
        }
    }
}
