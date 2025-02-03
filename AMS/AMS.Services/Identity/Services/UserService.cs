using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Identity;
using AMS.Interfaces.Mail;
using AMS.MODELS.Identity.User;
using AMS.MODELS.Models.Mail;
using AMS.MODELS.MODELS.SettingModels.Identity.User;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SHARED.Constants.Authorization;
using AMS.SHARED.Constants.Identity;
using AMS.SHARED.Enums.AMS;
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
using AMS.MODELS.Filters;
using AMS.MODELS.Session;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;


namespace AMS.SERVICES.Identity.Services
{
    public class UserService(
        AMSContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<SecuritySettings> securitySettings,
        IMailService mailService,
        IEmailTemplateService emailTemplateService,
        IJobService job,
        IRoleService roleService)
        : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly AMSContext _db = db;
        private readonly SecuritySettings _securitySettings = securitySettings.Value;
        private readonly IMailService _mailService = mailService;
        private readonly IJobService _job = job;
        private readonly IRoleService _roleService = roleService;
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;

        public async Task<UserDetailsDto> GetAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userManager
                .FindByIdAsync(userId.ToString())
                .ConfigureAwait(false);

            _ = user ?? throw new NotFoundException("User not found");

            return user.Adapt<UserDetailsDto>();
        }

        public async Task<ApplicantUserResponse> GetApplicantUserMe(ClaimsPrincipal claims)
        {
            var user = await GetUserFromClaimsAsync(claims);
            var applicant = await _db.Applicants.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);
            var response = new ApplicantUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = applicant.FullName ?? "",
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                ApplicantId = applicant.Id
            };
            return response;
        }
        public async Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException("User Not Found.");

            bool isAdmin = await _userManager.IsInRoleAsync(user, AMSRoles.Admin);
            if (isAdmin)
            {
                throw new ConflictException("Administrators Profile's Status cannot be toggled");
            }

            user.IsActive = request.ActivateUser;

            await _userManager.UpdateAsync(user);
        }

        public async Task<PaginationResponse<UserDetailsDto>> GetUserByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _db.Users
                 .Include(x => x.UserRoles)
                 .AsQueryable();

            var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
                    .LazyFilters(request)
                    .LazyOrderBy(request)
                    .LazySkipTake(request)
                    .Where(x => x.UserTypeEid == (int)UserType.Admin)
                    .Select(user => new UserDetailsDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault(),
                        ProfilePictureUrl = user.ProfilePictureUrl,
                        IsActive = user.IsActive,
                        IsEmailConfirmed = user.EmailConfirmed,
                    })
                    .ToListAsync(ct)
                    .ConfigureAwait(false)
                          :
                await query.AsNoTracking()
                    .LazySearch(request.GlobalFilter, "Username")
                    .Where(x => x.UserTypeEid == (int)UserType.Admin)
                    .Select(user => new UserDetailsDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault(),
                        ProfilePictureUrl = user.ProfilePictureUrl,
                        IsActive = user.IsActive,
                        IsEmailConfirmed = user.EmailConfirmed,
                    })
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

            return new PaginationResponse<UserDetailsDto>
            {
                Data = result.Adapt<List<UserDetailsDto>>(),
                Total = await query.CountAsync(ct),
            };
        }

        public async Task<CreateAdminUserResponse> CreateUserAsync(CreateAdminUserRequest user, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(user);
            var userEntity = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
            };
            var result = await _userManager.CreateAsync(userEntity);
            if (!result.Succeeded)
            {
                throw new AMSException(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
            }
            // var role = await _roleManager.FindByIdAsync(user.roleId.ToString()) ?? throw new NotFoundException("role not found");
            // await _userManager.AddToRoleAsync(userEntity, role.Name!);
            return new CreateAdminUserResponse(userEntity.Id);
        }
        public async Task<ApplicationUser> GetUserFromClaimsAsync(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims).ConfigureAwait(false);
            _ = user ?? throw new NotFoundException("User not found");
            return user;
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
        public async Task<string> CreateAdminUserAsync(CreateAdminUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                throw new BadRequestException("Email already exists");
            }

            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                throw new BadRequestException("Username already taken");
            }
            var user = new ApplicationUser()
            {
                Email = request.Email,
                UserName = GenerateUserName(request.UserName),
                UserTypeEid = (int)UserType.Admin
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Validation error has occurred");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, request.role);
            if (!roleResult.Succeeded)
            {
                throw new BadRequestException("Could 'nt add role to user");
            }
            var emailVerificationUri = await GenerateEmailVerificationLinkAsync(user, new Uri("http://localhost:4200/create-password"), new List<KeyValuePair<string, string>>());
            var eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri
            };

            var mailRequest = new MailRequest(
                // To user mail
                [user.Email],
                //subject
                "Confirm Registration",
                //body
                _emailTemplateService.GenerateEmailTemplate("email-confirmation-user", eMailModel));
            // fire and forget pattern so that's why we are sending cancellation.none token  
            _job.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
            //await _mailService.SendAsync(mailRequest, CancellationToken.None);
            var messages = new List<string>
            {
                $"User {user.UserName} Registered.",
                $"Please check {user.Email} to verify your account!"
            };
            return string.Join(Environment.NewLine, messages);
        }
        public async Task<string> CreateAsync(CreateUserRequest request, string origin)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (await _db.Applicants.AnyAsync(x => x.Cnic == request.Cnic))
                throw new BadRequestException($"CNIC {request.Cnic} Already registered ");
            if (userExists != null)
                throw new BadRequestException("Email already exists!, try different email");

            // create  user from request
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = GenerateUserName(request.FullName),
                IsActive = true,
                UserTypeEid = ((int)UserType.Applicant),
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new AMSException("Validation errors occurred");
            }
            // we have currently not seeded any role, will later seed some roles
            //await _userManager.AddToRoleAsync(user,AMSRoles.Applicant);

            var messages = new List<string> { $"User {user.UserName} Registered." };
            if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
            {
                // generate email verification uri
                string emailVerificationUri = await GetApplicantUserEmailVerificationUriAsync(user, origin, request.Cnic, request.FullName);
                RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Url = emailVerificationUri
                };

                var mailRequest = new MailRequest(
               // To user mail
               [user.Email],
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
        private async Task<string> GetApplicantUserEmailVerificationUriAsync(ApplicationUser user, string origin, string cnic, string fullName)
        {
            // server origin
            //const string route = "api/users/confirm-email/";
            //var endpointUri = new Uri(string.Concat($"{origin}/", route));
            //const string route = "api/users/confirm-email/";

            const string clientOrigin = "http://localhost:3000/verify-email/";
            var queryParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(QueryStringKeys.Cnic, cnic),
                new KeyValuePair<string, string>(QueryStringKeys.FullName, fullName)
            };

            return await GenerateEmailVerificationLinkAsync(user, new Uri(clientOrigin), queryParams);
        }

        private async Task<string> GenerateEmailVerificationLinkAsync(ApplicationUser user, Uri origin, List<KeyValuePair<string, string>> queryParams)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.ASCII.GetBytes(code));
            queryParams.Add(new KeyValuePair<string, string>(QueryStringKeys.Code, code));
            queryParams.Add(new KeyValuePair<string, string>(QueryStringKeys.UserId, user.Id.ToString()));
            return QueryHelpers.AddQueryString(origin.ToString(), queryParams!);
        }
        private static string GenerateUserName(string fullName)
            => $"{Regex.Replace(fullName.ToLower(), @"\s+", "")}{Random.Shared.NextInt64(9999)}";

        private async Task CreateApplicantAsync(ApplicationUser user, string cnic, string fullName)
        {
            var applicant = new Applicant
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                Cnic = cnic,
                FullName = fullName,
            };
            await _db.Applicants.AddAsync(applicant);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteUser(int id, CancellationToken ct)
        {
            var result = await _userManager.Users
                               .Where(x => x.Id == id).FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"user doesn't exist with id: {id}");
            }
            await _userManager.DeleteAsync(result);
        }
        public async Task<UpdateUserResponse> Updateuser(MODELS.MODELS.SettingModels.Identity.User.UpdateUserRequest Request,
              CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var user = await _userManager.Users.Where(x => x.Id == Request.Id).FirstOrDefaultAsync() ?? throw new NotFoundException($"user doesn't exist with id: {Request.Id}");
            user.UserName = Request.Username;
            user.Email = Request.Email;

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove existing roles if any
            if (currentRoles.Any())
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                var AddRole = await _userManager.AddToRoleAsync(user, Request.role);

                await _userManager.UpdateAsync(user);
               
            }
            return user.Adapt<UpdateUserResponse>();
        }
        public async Task<string> ConfirmEmailAsync(int userId, string code, string cnic, string fullName, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(user => user.Id == userId && !user.EmailConfirmed).FirstOrDefaultAsync(cancellationToken);
            _ = user ?? throw new AMSException("An error occured while confirming email");
            var result = await _userManager.ConfirmEmailAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)));
            if (result.Succeeded)
            {
                await CreateApplicantAsync(user, cnic, fullName);
                return $"Account Confirmed for E-Mail {user.Email}.";
            }
            else
            {
                throw new AMSException($"An error occurred while confirming {user.Email}");
            }
        }



        public async Task<string> ConfirmUserMailAsync(int userId, string code, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(user => user.Id == userId && !user.EmailConfirmed).FirstOrDefaultAsync(cancellationToken);
            _ = user ?? throw new AMSException("An error occured while confirming email");
            var result = await _userManager.ConfirmEmailAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)));
            if (result.Succeeded)
                return $"Account Confirmed for E-Mail {user.Email}.";
            throw new AMSException($"An error occurred while confirming {user.Email}");
        }

        public async Task<string> ConfirmEmailAndSetPassword(ConfirmMailAndResetPasswordDto request, int userId, string code, CancellationToken ct)
        {
            var message = await ConfirmUserMailAsync(userId, code, ct);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is not ApplicationUser)
            {
                throw new NotFoundException("User Not found!");
            }
            var result = await _userManager.AddPasswordAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new AMSException("An error occurred while setting password", result.GetErrors());
            }
            user.IsActive = true;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return "Password Set Successfully!";
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
