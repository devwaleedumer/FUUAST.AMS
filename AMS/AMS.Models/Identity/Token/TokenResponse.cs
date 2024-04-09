using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Identity.Token
{

    public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}
