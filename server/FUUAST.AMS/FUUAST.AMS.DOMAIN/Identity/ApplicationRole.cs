using FUUAST.AMS.DOMAIN.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Identity
{
    public class ApplicationRole : IdentityRole<int>, IBaseEntity<int>
    {
        public ApplicationRole() : base()
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            RoleModules = new HashSet<RoleModule>();
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            RoleModules = new HashSet<RoleModule>();
        }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        public virtual ICollection<RoleModule> RoleModules { get; set; }
    }
}
