using System.Security.Claims;


namespace AMS.SHARED.Interfaces.CurrentUser
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUser(ClaimsPrincipal user);

        void SetCurrentUserId(int userId);
    }
}
