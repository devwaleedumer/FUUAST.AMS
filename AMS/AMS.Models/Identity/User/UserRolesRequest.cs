using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Identity.User
{
    public class UserRolesRequest
    {
        public List<UserRoleDto> UserRoles { get; set; } = new();
    }
}
