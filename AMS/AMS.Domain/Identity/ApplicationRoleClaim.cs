using AMS.DOMAIN.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace AMS.DOMAIN.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<int>, IBaseEntity<int>
    {
        public virtual ApplicationRole Role { get; set; } = null!;

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
