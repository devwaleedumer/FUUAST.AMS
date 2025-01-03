using AMS.DOMAIN.Base;
using Microsoft.AspNetCore.Identity;

namespace AMS.DOMAIN.Identity;

public class ApplicationUserRole : IdentityUserRole<int>
{
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual ApplicationRole Role { get; set; } = default!;
}