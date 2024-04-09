using AMS.Shared.Models.FileUpload;

namespace AMS.Shared.Interfaces.FileUpload
{
    public interface IFileUploadService
    {
        Task<FileUploadResult> UploadAsync(FileUploadRequest request);
        bool DeleteFile(string url);
    }
}
