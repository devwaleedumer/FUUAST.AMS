using AMS.SERVICES.DataService;
using AMS.SERVICES.IDataService;
using AMS.SERVICES.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers.Identity
{
    [Authorize]
    public class UsersController(IUserService _service,  IProgramService programService) : BaseApiController
    {
        private readonly IProgramService _programService = programService;

        [HttpGet("me")]
        public async Task<IActionResult> GetUser() => Ok(await _service.GetUserFromClaimsAsync(HttpContext.User));

        [HttpGet("{userId}/programs")]
        public async Task<IActionResult> GetProgramByApplicantId([FromRoute] int userId, CancellationToken ct) => Ok(await _programService.GetProgramByApplicantId(userId, ct));

    }
}



     
