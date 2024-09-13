using AMS.DOMAIN.Identity;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace AMS.SERVICES.DataService
{
    public class UploadImageService : IUploadImageService
    {

        private readonly ICurrentUser _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public UploadImageService(UserManager<ApplicationUser> userManager, ICurrentUser currentUser)
        {

            _userManager = userManager;
            _currentUser = currentUser;

        }
        

        Task<string> IUploadImageService.UploadProfilePicture(IFormFile picture)
        {
            throw new NotImplementedException();
        }
    }
}
   

        

    



