using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Identity
{
    public class RoleModulePermission
    {
        public int Id { get; set; }
        public int RoleModuleId { get; set; }
        public string? PermissionValue { get; set; }

        public virtual RoleModule RoleModule { get; set; } = null!;
    }
}
