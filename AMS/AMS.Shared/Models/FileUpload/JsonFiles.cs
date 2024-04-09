namespace AMS.Shared.Models.FileUpload
{
    public class JsonFiles
    {
        public FileUploadResponse[] files;

        public JsonFiles(List<FileUploadResponse> filesList)
        {
            files = new FileUploadResponse[filesList.Count];
            for (int i = 0; i < filesList.Count; i++)
            {
                files[i] = filesList.ElementAt(i);
            }
        }
    }
}
