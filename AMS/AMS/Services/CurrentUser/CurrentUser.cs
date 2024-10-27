using AMS.SHARED.Interfaces.CurrentUser;
using System.Security.Claims;

namespace AMS.Services.CurrentUser
{
    public class CurrentUser : ICurrentUser, ICurrentUserInitializer
    {
        private ClaimsPrincipal? _user;

        public string? Name => _user?.GetFullName();

        private int? _userId = null;

        public int? GetUserId() =>
            IsAuthenticated()
                ? _user?.GetUserId()
                : _userId;

        public string? GetUserEmail() =>
            IsAuthenticated()
                ? _user?.GetEmail()
                : string.Empty;

        public bool IsAuthenticated() =>
            _user?.Identity?.IsAuthenticated is true;

        public bool IsInRole(string role) =>
            _user?.IsInRole(role) is true;

        public IEnumerable<Claim>? GetUserClaims() =>
            _user?.Claims;

        public void SetCurrentUser(ClaimsPrincipal user)
        {
            if (_user != null)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            _user = user;
        }

        public void SetCurrentUserId(int userId)
        {
            if (_userId != null)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }
            _userId = userId;
        }
    }
}
