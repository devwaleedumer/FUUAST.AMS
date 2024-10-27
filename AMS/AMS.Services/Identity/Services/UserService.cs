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
using AMS.SHARED.Extensions;
using AMS.SHARED.Interfaces.Hangfire;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace AMS.SERVICES.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AMSContext _db;
        private readonly SecuritySettings _securitySettings;
        private readonly IMailService _mailService;
        private readonly IJobService _job;
        private readonly IEmailTemplateService _emailTemplateService;

        public UserService(
            AMSContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<SecuritySettings> securitySettings,
            IMailService mailService,
            IEmailTemplateService emailTemplateService,
            IJobService job
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _securitySettings = securitySettings.Value;
            _emailTemplateService = emailTemplateService;
            _mailService = mailService;
            _job = job;
        }

        public async Task<UserDetailsDto> GetAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException("user not found");

            return user.Adapt<UserDetailsDto>();
        }       
        
        public async Task<ApplicantUserResponse> GetUserFromClaimsAsync(ClaimsPrincipal claims)
        {
           var user  = await _userManager.GetUserAsync(claims).ConfigureAwait(false);
            _ = user ?? throw new NotFoundException("User not found");
            return user.Adapt<ApplicantUserResponse>();
          
        }

        public async Task<bool> ExistsWithEmailAsync(string email, int? exceptId = null)

        => await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;

       
        public async Task<bool> ExistsWithNameAsync(string name)

         => await _userManager.FindByNameAsync(name) is not null;

        
        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, int? exceptId = null)
        => await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
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
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists is ApplicationUser) throw new ConflictException("Email already exists!, try different email");

            // create  user from request
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = GenerateUserName(request.FullName),
                FullName = request.FullName,
                IsActive = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new AMSException("Validation errors occurred");
            }
            // we have currently not seeded any role, will later seed some roles
            //await _userManager.AddToRoleAsync(user,AMSRoles.Basic);

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
                // fire and forget pattern so that's why we are sending cancellation.none token  
                _job.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
                //await _mailService.SendAsync(mailRequest, CancellationToken.None);
                messages.Add($"Please check {user.Email} to verify your account!");
            }
            return string.Join(Environment.NewLine, messages);
        }

        //Private helpers
        private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.ASCII.GetBytes(code));
            //const string route = "api/users/confirm-email/";
            //var endpointUri = new Uri(string.Concat($"{origin}/", route));
            //const string route = "api/users/confirm-email/";
            const string clientOrigin = "http://localhost:3000/verify-email/";
            var endpointUri = new Uri(clientOrigin);
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
            return verificationUri;
        }

        private string GenerateUserName(string fullName)
            => $"{Regex.Replace(fullName.ToLower(), @"\s+","")}{Random.Shared.NextInt64(9999)}";


        public async Task<string> ConfirmEmailAsync(int userId, string code, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(user => user.Id == userId && !user.EmailConfirmed).FirstOrDefaultAsync(cancellationToken);
            _ = user ?? throw new AMSException("An error occured while confirming email");
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded
           ? string.Format("Account Confirmed for E-Mail {0}.", user.Email)
           : throw new AMSException(string.Format("An error occurred while confirming {0}", user.Email));

        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !user.EmailConfirmed) throw new AMSException("some error has occured");
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            const string route = "account/reset-password";
            var endPointUri = new Uri(string.Concat($"{origin}/", route));
            string passwordResetUri = QueryHelpers.AddQueryString(endPointUri.ToString(), "Token", code);
            var mail = new MailRequest(new List<string> { request.Email }, "Reset Password", $"Your Password Reset Token is '{code}'. You can reset your password using the {endPointUri} Endpoint.");
            return "Password Reset Mail has been sent to your authorized Email.";
        }
        public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email?.Normalize()!);

            // Don't reveal that the user does not exist
            _ = user ?? throw new AMSException("An Error has occurred!");

            var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.Password!);

            return result.Succeeded
                ? "Password Reset Successful!"
                : throw new AMSException("An Error has occurred!");
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new NotFoundException("User Not Found.");

            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            if (!result.Succeeded)
            {
                throw new AMSException("Change password failed", result.GetErrors());
            }
        }
    }



}
