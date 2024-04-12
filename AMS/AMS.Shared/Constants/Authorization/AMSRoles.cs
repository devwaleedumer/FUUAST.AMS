using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SHARED.Constants.Authorization
{
    public static class AMSRoles
    {
        public const string Admin = nameof(Admin);
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string Basic = nameof(Basic);
        public const string Applicant = nameof(Applicant);

        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
        Admin,
        Basic,
        Applicant,
        SuperAdmin
    });

        public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
    }
}
