using AMS.SHARED.Constants.Authorization;

namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.Email);

    public static string? GetFullName(this ClaimsPrincipal principal)
        => principal?.FindFirst(AMSClaims.Fullname)?.Value;

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
    
       => principal.FindFirstValue(ClaimTypes.MobilePhone);
   
    public static int GetUserId(this ClaimsPrincipal principal)
        => Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));

    public static string? GetImageUrl(this ClaimsPrincipal principal)
       => principal.FindFirstValue(AMSClaims.ProfileImageUrl);

    public static DateTimeOffset GetExpiration(this ClaimsPrincipal principal) =>
        DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(
            principal.FindFirstValue(AMSClaims.Expiration)));

   
}