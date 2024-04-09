using AMS.MODELS.Identity.Token;

namespace AMS.SERVICES.Identity.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}
