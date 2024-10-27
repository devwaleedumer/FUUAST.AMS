using AMS.DOMAIN.Identity;
using AMS.MODELS.Identity.Token;
using AMS.MODELS.MODELS.SettingModels.Identity.Jwt;
using AMS.MODELS.MODELS.SettingModels.Identity.User;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SHARED.Constants.Authorization;
using AMS.SHARED.Exceptions;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AMS.SERVICES.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecuritySettings _securitySettings;
        private readonly JwtSettings _jwtSettings;

        public TokenService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            IOptions<SecuritySettings> securitySettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _securitySettings = securitySettings.Value;
        }
        /// <summary>
        /// Get Security Tokens i.e Access and Refresh
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
                || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException("Authentication Failed.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedException("User Not Active. Please contact the administrator.");
            }

            if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
            {
                throw new UnauthorizedException("E-Mail not confirmed.");
            }


            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }

        /// <summary>
        /// Generate pair of Token refresh and access token along expiry time
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string? useremail = userPrincipal.GetEmail();
            var user = await _userManager.FindByEmailAsync(useremail!);
            if (user is null)
            {
                throw new UnauthorizedException("Authentication Failed.");
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid Refresh Token.");
            }

            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }

        /// <summary>
        /// Create Tokens
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ipAddress"></param>
        /// <returns>TokenResponse</returns>
        private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
        {
            string token = GenerateJwt(user, ipAddress);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            await _userManager.UpdateAsync(user);

            return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime.Value);
        }

        /// <summary>
        ///  Generate JWT security access token Access Token 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ipAddress"></param>
        /// <returns cref="string"></returns>
        private string GenerateJwt(ApplicationUser user, string ipAddress) =>
            GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

        /// <summary>
        /// Generate Claims for Application user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ipAddress"></param>
        /// <returns>Claims</returns>
        private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
            new List<Claim>
            {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(AMSClaims.Fullname, $"{user.FullName}"),
            new(AMSClaims.IpAddress, ipAddress),
            new(AMSClaims.ProfileImageUrl, user.ProfilePictureUrl ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            };

        /// <summary>
        /// Random Bas64 Encoded string generated using RandomNumberGenerator
        /// </summary>
        /// <returns>Base64EncodedString</returns>
        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Generates Encrypted Token 
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        ///  ClaimsPrincipal from expired Access Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns cref="ClaimsPrincipal">ClaimsPrinciple</returns>
        /// <exception cref="UnauthorizedException"></exception>
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException("Invalid Token.");
            }

            return principal;
        }

      /// <summary>
      /// Sets Auth token in cookie
      /// </summary>
      /// <param name="request">Credentials request</param>
      /// <param name="ipAddress">Ip Address of remote user</param>
      /// <param name="cancellationToken"></param>
      /// <param name="context">HTTPContext for current request</param>
      /// <returns></returns>
        public async Task<CookieTokenResponse> SetTokensCookieAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken, HttpContext context)
        {
            var token = await GetTokenAsync(request, ipAddress, cancellationToken);  
            SetTokensCookie(context, token);
            return new CookieTokenResponse(token.RefreshTokenExpiryTime);
        }

        /// <summary>
        /// Refresh Token cookies
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<CookieTokenResponse> setRefreshTokensCookieAsync(string ipAddress,HttpContext context)
        {
            string? refreshToken = context.Request.Cookies["refreshToken"];
            if ( string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedException("invalid tokens");
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.UtcNow);
            if (user is null)
            {
                throw new UnauthorizedException("Authentication Failed.");
            }
            var tokens =  await GenerateTokensAndUpdateUser(user, ipAddress);
            SetTokensCookie(context, tokens);
            return new CookieTokenResponse(tokens.RefreshTokenExpiryTime);
        }

        public void RemoveCookieTokens(HttpContext context)
        {
            context.Response.Cookies.Delete("refreshToken");
            context.Response.Cookies.Delete("token");

        }

        /// <summary>
        /// Generate Signing Credentials using jwt secrets
        /// </summary>
        /// <returns>SigningCredentials</returns>
        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Sets tokens in cookie
        /// </summary>
        /// <param name="token">Tokens Object </param>
        /// <param name="context">HttpContext</param>
        private void SetTokensCookie(HttpContext context, TokenResponse token)
        {
            context.Response.Cookies.Append("token", token.Token,
             new CookieOptions
             {
                 // Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                 Expires = DateTimeOffset.UtcNow.AddMinutes(2),
                 HttpOnly = true,
                 IsEssential = true,
                 Secure = true,
                 SameSite = SameSiteMode.None
             });
            context.Response.Cookies.Append("refreshToken", token.RefreshToken,
                new CookieOptions
                {
                    // Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
                    Expires = DateTimeOffset.UtcNow.AddMinutes(20),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }
    }
}
