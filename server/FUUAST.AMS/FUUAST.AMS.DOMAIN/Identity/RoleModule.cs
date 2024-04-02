using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Identity
{
    public class RoleModule
    {
        public RoleModule()
        {
            RoleModulePermissions = new HashSet<RoleModulePermission>();
        }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ModuleName { get; set; } = null!;

        public virtual ApplicationRole Role { get; set; } = null!;
        public virtual ICollection<RoleModulePermission> RoleModulePermissions { get; set; }
    }
}
