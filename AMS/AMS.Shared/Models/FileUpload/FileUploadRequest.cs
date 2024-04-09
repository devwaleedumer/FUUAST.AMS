using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AMS.Shared.Models.FileUpload
{
    public class FileUploadRequest
    {
        [BindNever]
        public HttpContext HttpContext { get; set; } = null!;
        public string? MainFolderName { get; set; }
        public string? SubFolderName { get; set; }
        public string? FolderName { get; set; }
        public bool NeedThumbnails { get; set; }
        public int? ThumbnailMaxWidth { get; set; }
        public bool NeedResizing { get; set; }
        public int? ResizeMaxWidth { get; set; }

        public List<IFormFile> Files { get; set; } = null!;
    }
}
