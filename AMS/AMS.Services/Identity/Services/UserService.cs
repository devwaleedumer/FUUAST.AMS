using AMS.DATA;
using AMS.DOMAIN.Identity;
using AMS.Interfaces.Mail;
using AMS.MODELS.Identity.User;
using AMS.MODELS.Models.Mail;
using AMS.MODELS.MODELS.SettingModels.Identity.User;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SHARED.Constants.Authorization;
using AMS.SHARED.Constants.Identity;
using AMS.SHARED.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;


namespace AMS.SERVICES.Identity.Services
{
    public class UserService :  IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AMSContext _db;
        private readonly SecuritySettings _securitySettings;
        private readonly IMailService _mailService;
        private readonly IEmailTemplateService _emailTemplateService;

        public UserService(
            AMSContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<SecuritySettings> securitySettings,
            IMailService mailService,
            IEmailTemplateService emailTemplateService

            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _securitySettings = securitySettings.Value;
            _emailTemplateService = emailTemplateService;
            _mailService = mailService;
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

        public async Task<string> CreateAsync(CreateUserRequest request, string origin)
        {
            // create  user from request
            var user = new ApplicationUser
            {
                Email = request.Email,
                FullName = request.FullName,
                IsActive = true,
            };  

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                throw new InternalServerException("Validation errors occured");
            }
            await _userManager.AddToRoleAsync(user,AMSRoles.Basic);

            var messages = new List<string> { string.Format("User {0} Registered.", user.FullName) };
            if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
            {
                // generate email verification uri
                string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
                RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
                {
                    Email = user.Email,
                    UserName = user.FullName,
                    Url = emailVerificationUri
                };

                var mailRequest = new MailRequest(
               // To user mail
               new List<string> { user.Email },
               //subject
               "Confirm Registration",
               //body
               _emailTemplateService.GenerateEmailTemplate("email-confirmation", eMailModel));
                await  _mailService.SendAsync(mailRequest, CancellationToken.None);
                messages.Add($"Please check {user.Email} to verify your account!");

            }
            return string.Join(Environment.NewLine, messages);


        }

        //Private helpers
        private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin) {

            string code =  await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.ASCII.GetBytes(code));
            const string route = "api/users/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
            return verificationUri;
        }

        public async Task<string> ConfirmEmailAsync(int userId, string code, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(user => user.Id == userId && !user.EmailConfirmed).FirstOrDefaultAsync(cancellationToken);
            _ = user ?? throw new InternalServerException("An error occured while confirming email");
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded
           ? string.Format("Account Confirmed for E-Mail {0}.", user.Email)
           : throw new InternalServerException(string.Format("An error occurred while confirming {0}", user.Email));

        }
    }
}
