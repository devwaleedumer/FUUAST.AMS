using System.Security.Claims;


namespace FUUAST.AMS.SHARED.SharedServices.CurrentUser
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUser(ClaimsPrincipal user);

        void SetCurrentUserId(string userId);
    }
}