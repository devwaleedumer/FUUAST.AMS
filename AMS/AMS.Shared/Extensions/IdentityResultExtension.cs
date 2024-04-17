﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SHARED.Extensions
{
    public static class IdentityResultExtensions
    {
        public static List<string> GetErrors(this IdentityResult result ) =>
            result.Errors.Select(e => e.Description.ToString()).ToList();
    }
}
