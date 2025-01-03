using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Identity.User
{
    public class ConfirmMailAndResetPasswordDto
    {
       public string Password { get; set; } = default!;
       public string ConfirmPassword { get; set; } = default!;
    }
}
