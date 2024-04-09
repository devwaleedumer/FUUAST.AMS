namespace AMS.Shared.Models.FileUpload
{
    public class FileUploadResponse
    {
        public string name { get; set; } = string.Empty;
        public long   size { get; set; }
        public string type { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string origionalFilePath { get; set; } = string.Empty;
        public string thumbnailUrl { get; set; } = string.Empty;
        public string  thumbnailFullPath { get; set; } = string.Empty;
        public string deleteUrl { get; set; } = string.Empty;
        public string deleteType { get; set; } = string.Empty;
    }
}
