using AMS.MODELS.Identity.User;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers.Identity
{

    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        public AccountController(
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
        /// <response code="409">User alread exists </response>
        /// <response code="500">If User Creation error occured</response>    
        /// <response code="422">Model validation error</response>    
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
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
        public async Task<IActionResult> ConfirmAccount(int userId, string code, CancellationToken cancellation)
        {
            return Ok(await _userService.ConfirmEmailAsync(userId, code, cancellation));
        }

        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";

    }
}
