using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Identity.User
{
    public class ToggleUserStatusRequest
    {
        public bool ActivateUser { get; set; }
        public int? UserId { get; set; }
    }
}
