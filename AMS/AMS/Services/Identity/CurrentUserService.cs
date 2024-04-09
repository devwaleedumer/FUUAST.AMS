
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;

//namespace Crogics.ERP.Web.Services.Identity
//{
//    public class CurrentUserService : ICurrentUserService
//    {
//        private readonly IHttpContextAccessor httpContextAccessor;
//        private readonly CERPSettings settings;

//        public CurrentUserService(IHttpContextAccessor httpContextAccessor, CERPSettings settings)
//        {
//            this.httpContextAccessor = httpContextAccessor;
//            this.settings = settings;
//        }

//        public int UserId
//        {
//            get
//            {
//                int userId;
//                if (int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out userId))
//                {
//                    return userId;
//                }
//                return 0;
//            }
//        }

//        public int EmployeeId
//        {
//            get
//            {
//                int id;
//                if (int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.EmployeeId), out id))
//                {
//                    return id;
//                }
//                return 0;
//            }
//        }

//        public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

//        public string? PhoneNumber => httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.PhoneNumber);

//        public string? FullName => httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.FullName);

//        public string? ProfilePictureUrl => string.IsNullOrEmpty(httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.ProfilePictureUrl)) ? this.settings.BaseUrl + "/img/user/user-5.jpg" :
//                                                                 this.settings.BaseUrl + "/" + httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.ProfilePictureUrl);

//        public List<string>? Roles
//        {
//            get
//            {
//                return httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Where(c => c.Type == ClaimTypes.Role).Select(item => item.Value).ToList();
//            }
//        }

//        public bool IsInRole(string role)
//        {

//            if (Roles == null)
//                return false;
//            else
//                return Roles.Contains(role);
//        }

//        public List<string>? Permissions
//        {
//            get
//            {
//                return httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Where(c => c.Type.StartsWith("Permissions")).Select(item => item.Value).ToList();
//            }
//        }

//        public int FiscalYearId
//        {
//            get
//            {
//                int id;
//                if (int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.FiscalYearId), out id))
//                {
//                    return id;
//                }
//                return 0;
//            }
//        }

//        public string? FiscalYearName => httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.FiscalYearName);

//        public bool HasPermission(string permission)
//        {

//            if (Permissions == null)
//                return false;
//            else
//                return Permissions.Contains(permission);
//        }
//    }
//}
