using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.MODELS.SettingModels.Identity.User
{
    public class SecuritySettings
    {
        public string? Provider { get; set; }
        public bool RequireConfirmedAccount { get; set; }
    }
   public class UpdateUserRequest
    { 
        public int Id { get; set; }
        public string Username { get; set; }
        public string role { get; set; }
        public string Email { get; set; }
    }
    public class UpdateUserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string role { get; set; }
        public string Email { get; set; }
    }
}
