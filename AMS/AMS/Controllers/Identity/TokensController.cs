using AMS.MODELS.Identity.Token;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace AMS.Controllers.Identity
{

    public class TokensController : BaseApiController
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService) => _tokenService = tokenService;

        /// <summary>
        /// Generate Jwt Tokens after authentication 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/tokens/get-token
        ///     {        
        ///       "email": "user@gmail.com",
        ///       "password": "P@assW0rD",     
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns>Token Response</returns>
        /// <response code="200" >Returns  success</response>
        /// <response code="500" >If User credential validation error occured</response>    
        [HttpPost("get-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<IActionResult> GetTokenAsync([FromBody]TokenRequest request, CancellationToken cancellation)
           =>  Ok(await _tokenService.SetTokensCookieAsync(request, GetIpAddress()!, cancellation, HttpContext));

        /// <summary>
        /// Verify refresh token and generate new tokens 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/tokens/refresh
        ///     {        
        ///       "Token": "xxxxxxx",
        ///       "RefreshToken": "xxxxxxx",     
        ///     }
        /// </remarks>
        /// <returns>Refresh Token Response</returns>
        /// <response code="200" >Returns  success</response>
        /// <response code="401" >Invalid Request</response>    
        [HttpGet("refresh")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> Refresh()
            =>  Ok(await _tokenService.setRefreshTokensCookieAsync(GetIpAddress()!,HttpContext));

        /// <summary>
        /// Logout Successfully logout user
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [ProducesResponseType(200)]
        public IActionResult RemoveTokens()
        {
            _tokenService.RemoveCookieTokens(HttpContext);
            return Ok();
        }

        // helper function
        private string? GetIpAddress() =>
       Request.Headers.ContainsKey("X-Forwarded-For")
           ? Request.Headers["X-Forwarded-For"]
           : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}