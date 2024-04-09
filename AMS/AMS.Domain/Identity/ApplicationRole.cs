using AMS.DOMAIN.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Identity
{
    public class ApplicationRole : IdentityRole<int>,IBaseEntity<int>
    {
        public string? Description { get; set; }

        public ApplicationRole(string name, string? description = null)
            : base(name)
        {
            Description = description;
            NormalizedName = name.ToUpperInvariant();
            this.RoleClaims = new HashSet<ApplicationRoleClaim>();
        }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
