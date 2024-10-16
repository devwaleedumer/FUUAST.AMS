using AMS.MODELS.Identity.Token;
using Microsoft.AspNetCore.Http;

namespace AMS.SERVICES.Identity.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);

        Task<CookieTokenResponse> SetTokensCookieAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken, HttpContext context);

        Task<CookieTokenResponse> setRefreshTokensCookieAsync(string ipAddress, HttpContext context);
    }
}
