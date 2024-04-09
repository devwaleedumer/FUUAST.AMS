
using AMS.Shared.Models.FileUpload;

namespace AMS.Shared.Interfaces.FileUpload
{
    public interface IFileUploadHelper
    {
        string StorageRootPath { get; }
        string StorageTempPath { get; }
        string UrlBase { get; }
        string DeleteUrl { get; }
        string DeleteType { get; }

        void SetCustomStoragePaths(string? mainFolder = null, string? subFolder = null);
        void SetCustomFolder(string forlderName);
        string GetFullPath(string fileUrl);
        string DeleteFile(string file);
        JsonFiles GetFileList();
        FileUploadResponse UploadResult(string FileName, int fileSize, string FileFullPath);
        string CheckThumb(string type, string FileName);
    }
}
