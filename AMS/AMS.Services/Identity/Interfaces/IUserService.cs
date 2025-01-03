using AMS.DOMAIN.Identity;
using AMS.MODELS.Identity.User;
using System.Security.Claims;
using AMS.MODELS.Filters;


namespace AMS.SERVICES.Identity.Interfaces
{
    public interface IUserService
    {
        //Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);
        Task<bool> ExistsWithNameAsync(string name);
        Task<string> ConfirmUserMailAsync(int userId, string code, CancellationToken cancellationToken);
        Task<CreateAdminUserResponse> CreateUserAsync(CreateAdminUserRequest user, CancellationToken ct);
        Task<bool> ExistsWithEmailAsync(string email, int? exceptId = null);
        Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, int? exceptId = null);
        Task<ApplicantUserResponse> GetApplicantUserMe(ClaimsPrincipal claims);
        Task<PaginationResponse<UserDetailsDto>> GetUserByFilter(LazyLoadEvent request, CancellationToken ct);
        Task<ApplicationUser> GetUserFromClaimsAsync(ClaimsPrincipal claims);
        Task<UserDetailsDto> GetAsync(int userId, CancellationToken cancellationToken);
        Task<string> CreateAdminUserAsync(CreateAdminUserRequest request);
        //Task<int> GetCountAsync(CancellationToken cancellationToken);
        Task<string> ConfirmEmailAndSetPassword(ConfirmMailAndResetPasswordDto request, int userId, string code, CancellationToken ct);
        //Task<UserDetailsDto> GetAsync(int userId, CancellationToken cancellationToken);

        //Task<List<UserRoleDto>> GetRolesAsync(int userId, CancellationToken cancellationToken);
        //Task<string> AssignRolesAsync(int userId, UserRolesRequest request, CancellationToken cancellationToken);

        Task<List<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken);
        Task<bool> HasPermissionAsync(int userId, string permission, CancellationToken cancellationToken = default);
        ////Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);

        Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

        ////Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
        Task<string> CreateAsync(CreateUserRequest request, string origin);
        //Task UpdateAsync(UpdateUserRequest request, string userId);

        Task<string> ConfirmEmailAsync(int userId, string code, string cnic, string fullName, CancellationToken cancellationToken);
        //Task<string> ConfirmPhoneNumberAsync(int userId, string code);

        Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<string> ResetPasswordAsync(ResetPasswordRequest request);
        Task ChangePasswordAsync(ChangePasswordRequest request, string userId);
    }
}
