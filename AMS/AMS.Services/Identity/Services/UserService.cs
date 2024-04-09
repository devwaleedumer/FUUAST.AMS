using AMS.DATA;
using AMS.DOMAIN.Identity;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SHARED.Constants.Authorization;
using AMS.SHARED.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AMS.SERVICES.Identity.Services
{
    public class UserService :  IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AMSContext _db;

        public UserService(
            AMSContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager

            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public async Task<bool> ExistsWithEmailAsync(string email, int? exceptId = null)
        
        =>      await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
        

        public async Task<bool> ExistsWithNameAsync(string name)
        
         =>    await _userManager.FindByNameAsync(name) is not null;
        

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, int? exceptId = null)
        =>      await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
        public async Task<bool> HasPermissionAsync(int userId, string permission, CancellationToken cancellationToken)
        {
            var permissions = await GetPermissionsAsync(userId, cancellationToken);
        

            return permissions?.Contains(permission) ?? false;
        }

        public async Task<List<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            _ = user ?? throw new UnauthorizedException("Authentication Failed.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            foreach (var role in await _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name!))
                .ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _db.RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == AMSClaims.Permission)
                    .Select(rc => rc.ClaimValue!)
                    .ToListAsync(cancellationToken));
            }

            return permissions.Distinct().ToList();
        }


    }
}
