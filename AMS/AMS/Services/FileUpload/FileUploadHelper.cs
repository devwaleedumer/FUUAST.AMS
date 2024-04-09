
using AMS.Shared.Controls;
using AMS.Shared.Interfaces.FileUpload;
using AMS.Shared.Models.FileUpload;

namespace Crogics.ERP.Web.Services.FileUpload
{
    public class FileUploadHelper : IFileUploadHelper
    {
        //private fields
        private string TEMP_PATH;
        private string FILE_DIR_PATH = @"UploadedFiles\somefiles";
        private string URL_BASE = "/UploadedFiles/somefiles/";
        private const string DELETE_URL = "/FileUpload/DeleteFile/?file=";
        private const string DELETE_TYPE = "GET";

        private string _storageRootPath;
        private string _storageTempPath;
        //private readonly fields
        private readonly IWebHostEnvironment _env;
        //readonly properties
        public string StorageRootPath => _storageRootPath;
        public string StorageTempPath => _storageTempPath;
        public string UrlBase => URL_BASE;
        public string DeleteUrl => DELETE_URL;
        public string DeleteType => DELETE_TYPE;

        public FileUploadHelper(IWebHostEnvironment env)
        {
            TEMP_PATH = @"somefiles";
            FILE_DIR_PATH = @"UploadedFiles\somefiles";
            URL_BASE = "/UploadedFiles/somefiles/";
            _env = env;
            _storageRootPath = Path.Combine(env.WebRootPath, FILE_DIR_PATH);
            _storageTempPath = Path.Combine(env.WebRootPath, TEMP_PATH);
        }

        /// <summary>
        /// Sets the custom storage paths given by user
        /// </summary>
        /// <param name="mainFolder"></param>
        /// <param name="subFolder"></param>
        public void SetCustomStoragePaths(string? mainFolder = null, string? subFolder = null)
        {
            if (mainFolder != null && subFolder == null)
            {
                TEMP_PATH = @"somefiles";
                FILE_DIR_PATH = @"UploadedFiles\" + mainFolder;
                URL_BASE = "/UploadedFiles/" + mainFolder + "/";
                _storageRootPath = Path.Combine(_env.WebRootPath, FILE_DIR_PATH);
            }
            else if (mainFolder != null && subFolder != null)
            {
                TEMP_PATH = @"somefiles";
                FILE_DIR_PATH = @"UploadedFiles\" + mainFolder + @"\" + subFolder;
                URL_BASE = "/UploadedFiles/" + mainFolder + "/" + subFolder + "/";
                _storageRootPath = Path.Combine(_env.WebRootPath, FILE_DIR_PATH);
            }
        }
        public void SetCustomFolder(string forlderName)
        {
            if (forlderName != string.Empty)
            {
                _storageRootPath = Path.Combine(_storageRootPath, forlderName);
                var uri = new System.Uri(_storageRootPath);
                var url = uri.AbsoluteUri + "/";
                URL_BASE = url.Split("wwwroot")[1];
            }
        }

        public string GetFullPath(string fileUrl)
        {
            var path = fileUrl.Replace("/", @"\");
            return _env.WebRootPath + path;
        }

        public string DeleteFile(string file)
        {
            string fullPath = Path.Combine(_storageRootPath, file);
            string thumbPath = "/" + file + "80x80.jpg";
            string partThumb1 = Path.Combine(_storageRootPath, "thumbs");
            string partThumb2 = Path.Combine(partThumb1, file + "80x80.jpg");

            if (File.Exists(fullPath))
            {
                //delete thumb 
                if (File.Exists(partThumb2))
                {
                    File.Delete(partThumb2);
                }
                File.Delete(fullPath);
                string succesMessage = "Ok";
                return succesMessage;
            }
            string failMessage = "Error Delete";
            return failMessage;
        }

        public JsonFiles GetFileList()
        {
            var r = new FileUploadResult();

            string fullPath = Path.Combine(_storageRootPath);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                foreach (FileInfo file in dir.GetFiles())
                {
                    int SizeInt = unchecked((int)file.Length);
                    r.Results.Add(UploadResult(file.Name, SizeInt, file.FullName));
                }

            }
            JsonFiles files = new JsonFiles(r.Results);
            return files;
        }

        public FileUploadResponse UploadResult(string FileName, int fileSize, string FileFullPath)
        {
            string getType = MimeMapping.GetMimeMapping(FileFullPath);
            var result = new FileUploadResponse()
            {
                name = FileName,
                size = fileSize,
                type = getType,
                url = URL_BASE + FileName,
                deleteUrl = DELETE_URL + FileName,
                thumbnailUrl = CheckThumb(getType, FileName),
                deleteType = DELETE_TYPE
            };
            return result;
        }

        public string CheckThumb(string type, string FileName)
        {
            var splited = type.Split('/');
            if (splited.Length == 2)
            {
                string extansion = splited[1].ToLower();
                if (extansion.Equals("jpeg") || extansion.Equals("jpg") || extansion.Equals("png") || extansion.Equals("gif"))
                {
                    string thumbnailUrl = URL_BASE + "thumbs/" + Path.GetFileNameWithoutExtension(FileName) + $"80x80{Path.GetExtension(FileName)}";
                    return thumbnailUrl;
                }
                else
                {
                    if (extansion.Equals("octet-stream")) //Fix for exe files
                    {
                        return "/Content/Free-file-icons/48px/exe.png";

                    }
                    if (extansion.Contains("zip")) //Fix for exe files
                    {
                        return "/Content/Free-file-icons/48px/zip.png";
                    }
                    string thumbnailUrl = "/Content/Free-file-icons/48px/" + extansion + ".png";
                    return thumbnailUrl;
                }
            }
            else
            {
                return URL_BASE + "/thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "80x80.jpg";
            }
        }
    }
}
