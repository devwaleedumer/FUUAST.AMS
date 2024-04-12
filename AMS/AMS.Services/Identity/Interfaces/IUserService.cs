using AMS.MODELS.Identity.User;


namespace AMS.SERVICES.Identity.Interfaces
{
    public interface IUserService
    {
        //Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);
        Task<bool> ExistsWithNameAsync(string name);

        Task<bool> ExistsWithEmailAsync(string email, int? exceptId = null);
        Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, int? exceptId = null);

        //Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

        //Task<int> GetCountAsync(CancellationToken cancellationToken);

        //Task<UserDetailsDto> GetAsync(int userId, CancellationToken cancellationToken);

        //Task<List<UserRoleDto>> GetRolesAsync(int userId, CancellationToken cancellationToken);
        //Task<string> AssignRolesAsync(int userId, UserRolesRequest request, CancellationToken cancellationToken);

        Task<List<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken);
        Task<bool> HasPermissionAsync(int userId, string permission, CancellationToken cancellationToken = default);
        ////Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);

        //Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

        ////Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
        Task<string> CreateAsync(CreateUserRequest request, string origin);
        //Task UpdateAsync(UpdateUserRequest request, string userId);

        Task<string> ConfirmEmailAsync(int userId, string code, CancellationToken cancellationToken);
        ////Task<string> ConfirmPhoneNumberAsync(int userId, string code);

        //Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        //Task<string> ResetPasswordAsync(ResetPasswordRequest request);
        //Task ChangePasswordAsync(ChangePasswordRequest request, string userId);
    }
}
