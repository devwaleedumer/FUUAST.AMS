using Microsoft.AspNetCore.Http;

namespace AMS.SERVICES.IDataService
{
    public interface IUploadImageService
    {
        Task<string> UploadProfilePicture(IFormFile picture);
    }
}
