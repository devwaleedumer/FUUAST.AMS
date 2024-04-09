using AMS.SHARED.Constants.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace AMS.Authorization.Permissons
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = AMSPermission.NameFor(action, resource);
    }
}
