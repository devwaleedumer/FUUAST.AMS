using AMS.SERVICES.DataService;
using AMS.SERVICES.IDataService;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers.Identity
{
    public class UsersController(IUserService _service,  IProgramService programService) : BaseApiController
    {
        private readonly IProgramService _programService = programService;
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUser() => Ok(await _service.GetUserFromClaimsAsync(HttpContext.User));
        [Authorize]

        [HttpGet("{userId}/programs")]
        public async Task<IActionResult> GetProgramByApplicantId([FromRoute] int userId, CancellationToken ct) => Ok(await _programService.GetProgramByApplicantId(userId, ct));

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(int userId, string code,CancellationToken ct)
            => Ok(await _service.ConfirmEmailAsync(userId, code, ct));
    }
}



     
