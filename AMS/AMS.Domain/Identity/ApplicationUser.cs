using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
using AMS.SHARED.Enums.AMS;
using Microsoft.AspNetCore.Identity;


namespace AMS.DOMAIN.Identity
{
    public class ApplicationUser: IdentityUser<int>, IBaseEntity<int>
    {
        public ApplicationUser()
        {
            UserRoles = new HashSet<ApplicationUserRole>();
        }
        public string? ProfilePictureUrl { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int UserTypeEid  { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
