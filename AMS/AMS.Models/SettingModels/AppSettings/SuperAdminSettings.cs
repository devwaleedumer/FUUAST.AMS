using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.SettingModels.AppSettings
{
    public class SuperAdminSettings
    {

        [Required(ErrorMessage = "FullName in SuperAdminSettings is required")]
        public required string FullName { get; set; }
        [Required(ErrorMessage = "Email in SuperAdminSettings is required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password in SuperAdminSettings is required")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Username in SuperAdminSettings is required")]
        public required string Username { get; set; }

    }


}


