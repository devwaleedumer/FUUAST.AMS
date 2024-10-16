using AMS.MODELS.ApplicationForm.Applicant;
using AMS.SHARED.Enums.Shared;

namespace AMS.SERVICES.IDataService
{
    public interface ILocalFileStorageService
    {
        void Remove(string? path);
        Task<string> UploadAsync<T>(FileRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default) where T : class;
    }
}