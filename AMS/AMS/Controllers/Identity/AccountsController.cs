using AMS.MODELS.Identity.User;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers.Identity
{

    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        public AccountsController(
            IUserService userService    
            )
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates an Application User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/register
        ///     {        
        ///       "fullname": "Waleed Umer",
        ///       "email": "dev.waleedumer@gmail.com"        
        ///       "password": "xxxx",
        ///       "confirmPassword": "xxxx",
        ///     }
        /// </remarks>
        /// <param name="createUserRequest"></param>  
        ///  <returns>Sends confirmation mail and success or failure  messsage</returns>
        /// <response code="200">Returns the success or failure message </response>
        /// <response code="500">If User Creation error occured</response>    
        /// <response code="400">Model validation error</response>    
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<IActionResult> Register([FromBody]CreateUserRequest createUserRequest)
        {
            return Ok(await _userService.CreateAsync(createUserRequest, GetOriginFromRequest()));
        }

        /// <summary>
        ///  Confirms user mail and enable user account
        ///  </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <param name="cancellation"></param>
        /// <returns>Success or failure message</returns>
        /// <response code="200">Returns  success  message </response>
        /// <response code="500">If Token is expired </response>    
        [HttpGet("confirm-email")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] int userId,
                                                      [FromQuery] string code,
                                                      [FromQuery] string cnic,
                                                      [FromQuery] string fullName, CancellationToken ct)

            => Ok(await _userService.ConfirmEmailAsync(userId, code, cnic, fullName, ct));
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            return Ok(await _userService.ForgotPasswordAsync(request, GetOriginFromRequest()));
        }

        [HttpPost("confirm-mail-set-password")]
        public async Task<IActionResult> ConfirmMailAndSetPassword([FromBody] ConfirmMailAndResetPasswordDto request, [FromQuery] int userId, [FromQuery]  string code,CancellationToken ct)
        {
            return Ok( new {
                message = await _userService.ConfirmEmailAndSetPassword(request, userId, code, ct)
            });
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleUserActiveStatus ([FromBody]ToggleUserStatusRequest request,CancellationToken ct)
        {
            await _userService.ToggleStatusAsync(request, ct);
            return Ok();
        }
        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";

    }
}
