using AMS.Authorization.Permissons;
using AMS.MODELS.Filters;
using AMS.MODELS.Identity.User;
using AMS.SERVICES.IDataService;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SHARED.Constants.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers.Identity
{
    public class UsersController(IUserService userService,  IProgramService programService) : BaseApiController
    {
        private readonly IProgramService _programService = programService;
        private readonly IUserService _userService = userService;
        [Authorize]
        [HttpGet("applicant/me")]
        public async Task<IActionResult> GetUser() => Ok(await _userService.GetApplicantUserMe(HttpContext.User));
    
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateAdminUserRequest userRequest,
            CancellationToken cancellationToken)
        {
            var result = await _userService.CreateAdminUserAsync(userRequest);
            return Ok(new {
                message=result
            });
        }
       
        [Authorize]
        [HttpGet("{userId}/programs")]
        public async Task<IActionResult> GetProgramByApplicantId([FromRoute] int userId, CancellationToken ct) => Ok(await _programService.GetProgramByApplicantId(userId, ct));
        
        [MustHavePermission(AMSAction.View,AMSResource.Users)]
        [HttpPost("filter")]
        public async Task<IActionResult> GetAllAcademicYearByFilter([FromBody] LazyLoadEvent request, CancellationToken ct)
            => Ok(await _userService.GetUserByFilter(request, ct));
        
    }
}



     
