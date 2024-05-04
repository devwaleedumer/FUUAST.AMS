using AMS.DATA;
using AMS.DOMAIN.Identity;
using AMS.MODELS.SettingModels.AppSettings;
using AMS.SHARED.Constants.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AMS.DatabaseSeed
{
    internal class ApplicationDbSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomSeederRunner _seederRunner;
        private readonly ILogger<ApplicationDbSeeder> _logger;
        private readonly SuperAdminSettings _admin;

        public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
            CustomSeederRunner seederRunner,
            ILogger<ApplicationDbSeeder> logger, IOptions<SuperAdminSettings> admin)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _seederRunner = seederRunner;
            _logger = logger;
            _admin = admin.Value;
        }

        public async Task SeedDatabaseAsync(AMSContext dbContext, CancellationToken cancellationToken)
        {
            await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
            await _seederRunner.RunSeedersAsync(cancellationToken);
        }

        private async Task SeedRolesAsync(AMSContext dbContext)
        {
            foreach (string roleName in AMSRoles.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                    is not ApplicationRole role)
                {
                    // Create the role
                    _logger.LogInformation("Seeding {role} Role .", roleName);
                    role = new ApplicationRole(roleName, $"{roleName} Role for Tenant");
                    await _roleManager.CreateAsync(role);
                }

                // Assign permissions
                if (roleName == AMSRoles.Basic)
                {
                    await AssignPermissionsToRoleAsync(dbContext, AMSPermissions.Basic, role);
                }
                else if (roleName == AMSRoles.Admin)
                {
                    await AssignPermissionsToRoleAsync(dbContext, AMSPermissions.Admin, role);
                }
            }
        }

        private async Task AssignPermissionsToRoleAsync(AMSContext dbContext, IReadOnlyList<AMSPermission> permissions, ApplicationRole role)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == AMSClaims.Permission && c.Value == permission.Name))
                {
                    _logger.LogInformation("Seeding {role} Permission '{permission}'.", role.Name, permission.Name);
                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = AMSClaims.Permission,
                        ClaimValue = permission.Name,
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {

            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _admin.Email)
                is not ApplicationUser adminUser)
            {
                adminUser = new ApplicationUser
                {
                    FullName = _admin.FullName,
                    Email = _admin.Email,
                    UserName = _admin.Username,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _admin.Email?.ToUpperInvariant(),
                    NormalizedUserName = _admin.Username.ToUpperInvariant(),
                    IsActive = true,

                };

                _logger.LogInformation("Seeding Default Admin User.");
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, _admin.Password);
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, AMSRoles.Admin))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User .");
                await _userManager.AddToRoleAsync(adminUser, AMSRoles.Admin);
            }
        }
    }
}
