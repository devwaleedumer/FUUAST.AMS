using AMS.DOMAIN.Identity;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(
            RoleManager<ApplicationRole> roleManager
            )
        {
            _roleManager = roleManager;
        }
        public async Task<bool> ExistsAsync(string roleName, int? excludeId) =>
        await _roleManager.FindByNameAsync(roleName)
         is ApplicationRole existingRole
         && existingRole.Id != excludeId;
    }
}
