namespace AMS.MODELS.Identity.Token
{

    public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}
