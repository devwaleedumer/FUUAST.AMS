//using AMS.DATA;
//using AMS.DOMAIN.Identity;
//using AMS.SHARED.Constants.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace AMS.DatabaseSeed
//{
//    internal class ApplicationDbSeeder
//    {
//        private readonly RoleManager<ApplicationRole> _roleManager;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly CustomSeederRunner _seederRunner;
//        private readonly ILogger<ApplicationDbSeeder> _logger;

//        public ApplicationDbSeeder( RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//            _seederRunner = seederRunner;
//            _logger = logger;
//        }

//        public async Task SeedDatabaseAsync(AMSContext dbContext, CancellationToken cancellationToken)
//        {
//            await SeedRolesAsync(dbContext);
//            await SeedAdminUserAsync();
//            await _seederRunner.RunSeedersAsync(cancellationToken);
//        }

//        private async Task SeedRolesAsync(AMSContext dbContext)
//        {
//            foreach (string roleName in AMSRoles.DefaultRoles)
//            {
//                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
//                    is not ApplicationRole role)
//                {
//                    // Create the role
//                    _logger.LogInformation("Seeding {role} Role .", roleName);
//                    role = new ApplicationRole(roleName, $"{roleName} Role for Tenant");
//                    await _roleManager.CreateAsync(role);
//                }

//                // Assign permissions
//                if (roleName == AMSRoles.Basic)
//                {
//                    await AssignPermissionsToRoleAsync(dbContext, AMSPermissions.Basic, role);
//                }
//                else if (roleName == AMSRoles.Admin)
//                {
//                    await AssignPermissionsToRoleAsync(dbContext, AMSPermissions.Admin, role);

//                    if (_currentTenant.Id == MultitenancyConstants.Root.Id)
//                    {
//                        await AssignPermissionsToRoleAsync(dbContext, AMSPermissions.Root, role);
//                    }
//                }
//            }
//        }

//        private async Task AssignPermissionsToRoleAsync(AMSContext dbContext, IReadOnlyList<AMSPermission> permissions, ApplicationRole role)
//        {
//            var currentClaims = await _roleManager.GetClaimsAsync(role);
//            foreach (var permission in permissions)
//            {
//                if (!currentClaims.Any(c => c.Type == AMSClaims.Permission && c.Value == permission.Name))
//                {
//                    _logger.LogInformation("Seeding {role} Permission '{permission}' for '{tenantId}' Tenant.", role.Name, permission.Name, _currentTenant.Id);
//                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
//                    {
//                        RoleId = role.Id,
//                        ClaimType = AMSClaims.Permission,
//                        ClaimValue = permission.Name,
//                    });
//                    await dbContext.SaveChangesAsync();
//                }
//            }
//        }

//        private async Task SeedAdminUserAsync()
//        {
//            if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
//            {
//                return;
//            }

//            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
//                is not ApplicationUser adminUser)
//            {
//                string adminUserName = $"{_currentTenant.Id.Trim()}.{AMSRoles.Admin}".ToLowerInvariant();
//                adminUser = new ApplicationUser
//                {
//                    FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
//                    LastName = AMSRoles.Admin,
//                    Email = _currentTenant.AdminEmail,
//                    UserName = adminUserName,
//                    EmailConfirmed = true,
//                    PhoneNumberConfirmed = true,
//                    NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
//                    NormalizedUserName = adminUserName.ToUpperInvariant(),
//                    IsActive = true
//                };

//                _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
//                var password = new PasswordHasher<ApplicationUser>();
//                adminUser.PasswordHash = password.HashPassword(adminUser, MultitenancyConstants.DefaultPassword);
//                await _userManager.CreateAsync(adminUser);
//            }

//            // Assign role to user
//            if (!await _userManager.IsInRoleAsync(adminUser, AMSRoles.Admin))
//            {
//                _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
//                await _userManager.AddToRoleAsync(adminUser, AMSRoles.Admin);
//            }
//        }
//    }
