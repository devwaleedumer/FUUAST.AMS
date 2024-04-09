namespace AMS.Shared.Models.FileUpload
{
    public class FileUploadResult
    {
        public FileUploadResult()
        {
            Results = new List<FileUploadResponse>();
        }
        public List<FileUploadResponse> Results { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; }
    }
}
