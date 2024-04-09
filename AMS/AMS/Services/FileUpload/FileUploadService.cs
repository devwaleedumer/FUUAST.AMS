using AMS.Shared.Controls;
using AMS.Shared.Interfaces.FileUpload;
using AMS.Shared.Models.FileUpload;
using ImageMagick;
using SkiaSharp;



namespace Crogics.ERP.Web.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadHelper _filesHelper;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IFileUploadHelper filesHelper, ILogger<FileUploadService> logger)
        {
            _filesHelper = filesHelper;
            _logger = logger;
        }

        public async Task<FileUploadResult> UploadAsync(FileUploadRequest request)
        {
            var result = new FileUploadResult();

            var partialFileName = request.HttpContext.Request.Headers["X-File-Name"];
            if (string.IsNullOrWhiteSpace(partialFileName))
            {
                await UploadWholeFileAsync(request, result);
            }
            else
            {
                UploadPartialFile(request, partialFileName);
            }
            return result;
        }
        public bool DeleteFile(string url)
        {
            try
            {
                var filePath = _filesHelper.GetFullPath(url);
                if (filePath != null)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, @"Exception in EmployeesController/Get-WizardTraining" + ex.Message);
                throw;
            }
        }
        #region Private Methods
        private static readonly HashSet<string> _allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".gif",
            ".jpeg",
            ".jpg",
            ".png",
            ".pdf"
        };

        private static readonly HashSet<string> _nonResizableExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf"
        };

        private async Task UploadWholeFileAsync(FileUploadRequest request, FileUploadResult result)
        {
            string thumbsFullPath = string.Empty;
            string resizedFullPath = string.Empty;
            string origionalFullPath = string.Empty;
            int THUMB_WIDTH = 80;
            int THUMB_HEIGHT = 80;
            int NORMAL_IMAGE_MAX_WIDTH = 540;
            string THUMBS_FOLDER_NAME = "thumbs";
            if (request.ThumbnailMaxWidth.HasValue)
                THUMB_WIDTH = THUMB_HEIGHT = request.ThumbnailMaxWidth.Value;
            if (request.ResizeMaxWidth.HasValue)
                NORMAL_IMAGE_MAX_WIDTH = request.ResizeMaxWidth.Value;

            try
            {
                //seting storage root paths
                _filesHelper.SetCustomStoragePaths(request.MainFolderName, request.SubFolderName);
                if (request.FolderName != null)
                {
                    _filesHelper.SetCustomFolder(request.FolderName);
                }
                // Ensure the storage root exists.
                if (!Directory.Exists(_filesHelper.StorageRootPath))
                {
                    Directory.CreateDirectory(_filesHelper.StorageRootPath);
                }

                foreach (var file in request.Files)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_allowedExtensions.Contains(extension))
                    {
                        // This is not a supported image type.
                        throw new InvalidOperationException($"Unsupported image type: {extension}. The supported types are: {string.Join(", ", _allowedExtensions)}");
                    }
                    if (_nonResizableExtensions.Contains(extension))
                    {
                        var fullPath = Path.Combine(_filesHelper.StorageRootPath, Path.GetFileName(file.FileName));
                        if (File.Exists(fullPath))
                        {
                            var exFilename = Path.GetFileNameWithoutExtension(file.FileName) + "_1" + extension;
                            fullPath = Path.Combine(_filesHelper.StorageRootPath, exFilename);
                        }
                        using (var fs = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                        origionalFullPath = fullPath;
                    }
                    else
                    {
                        if (file.Length > 0L)
                        {
                            var fullPath = Path.Combine(_filesHelper.StorageRootPath, Path.GetFileName(file.FileName));
                            if (File.Exists(fullPath))
                            {
                                var exFilename = Path.GetFileNameWithoutExtension(file.FileName) + "_1" + extension;
                                fullPath = Path.Combine(_filesHelper.StorageRootPath, exFilename);
                            }
                            using (var fs = new FileStream(fullPath, FileMode.Create))
                            {
                                await file.CopyToAsync(fs);
                            }
                            origionalFullPath = fullPath;
                            // Create an 80x80 thumbnail, and possibly resize the original if it exceeds an arbitrary max width.
                            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            var thumbName = $"{fileNameWithoutExtension}{THUMB_WIDTH}x{THUMB_HEIGHT}{extension}";
                            var thumbPath = Path.Combine(_filesHelper.StorageRootPath, THUMBS_FOLDER_NAME, thumbName);

                            // Create the thumnail directory if it doesn't exist.
                            if (request.NeedThumbnails)
                            {
                                var thumbsDir = Path.GetDirectoryName(thumbPath);
                                if (!Directory.Exists(thumbsDir))
                                {
                                    Directory.CreateDirectory(thumbsDir!);
                                }
                            }

                            // For GIFs, we have to use MagickNET. For JPEGs and PNGs, we can use SkiaSharp.
                            if (IsGif(extension))
                            {
                                if (request.NeedThumbnails)
                                {
                                    using (var thumbStream = MagickNetResizeImage(fullPath, destWidthPx: 80, destHeightPx: 80))
                                    using (var thumbFileStream = File.OpenWrite(thumbPath))
                                    {
                                        await thumbStream.CopyToAsync(thumbFileStream);
                                    }
                                    thumbsFullPath = thumbPath;
                                }
                                // If the image is wider than 540px, resize it so that it is 540px wide. Otherwise, upload a copy of the original.
                                using var originalImage = SKBitmap.Decode(fullPath);
                                if (request.NeedResizing && originalImage.Width > NORMAL_IMAGE_MAX_WIDTH)
                                {
                                    // Resize it so that the max width is 540px. Maintain the aspect ratio.
                                    var newHeight = originalImage.Height * NORMAL_IMAGE_MAX_WIDTH / originalImage.Width;

                                    var normalImageName = $"{fileNameWithoutExtension}{NORMAL_IMAGE_MAX_WIDTH}x{newHeight}{extension}";
                                    var normalImagePath = Path.Combine(_filesHelper.StorageRootPath, normalImageName);

                                    using (var normalImgStream = MagickNetResizeImage(fullPath, NORMAL_IMAGE_MAX_WIDTH, newHeight))
                                    using (var normalImgFileStream = File.OpenWrite(normalImagePath))
                                    {
                                        await normalImgStream.CopyToAsync(normalImgFileStream);
                                    }
                                    resizedFullPath = normalImagePath;
                                }
                            }
                            else
                            {
                                if (request.NeedThumbnails)
                                {
                                    using (var thumbStream = SkiaSharpResizeImage(fullPath, destWidthPx: THUMB_WIDTH, destHeightPx: THUMB_HEIGHT))
                                    using (var thumbFileStream = File.OpenWrite(thumbPath))
                                    {
                                        await thumbStream.CopyToAsync(thumbFileStream);
                                    }
                                    thumbsFullPath = thumbPath;
                                }
                                // If the image is wider than 540px, resize it so that it is 540px wide. Otherwise, upload a copy of the original.
                                using var originalImage = SKBitmap.Decode(fullPath);
                                if (request.NeedResizing && originalImage.Width > NORMAL_IMAGE_MAX_WIDTH)
                                {
                                    // Resize it so that the max width is 540px. Maintain the aspect ratio.
                                    var newHeight = originalImage.Height * NORMAL_IMAGE_MAX_WIDTH / originalImage.Width;

                                    var normalImageName = $"{fileNameWithoutExtension}{NORMAL_IMAGE_MAX_WIDTH}x{newHeight}{extension}";
                                    var normalImagePath = Path.Combine(_filesHelper.StorageRootPath, normalImageName);

                                    using (var normalImgStream = SkiaSharpResizeImage(fullPath, NORMAL_IMAGE_MAX_WIDTH, newHeight))
                                    using (var normalImgFileStream = File.OpenWrite(normalImagePath))
                                    {
                                        await normalImgStream.CopyToAsync(normalImgFileStream);
                                    }
                                    resizedFullPath = normalImagePath;
                                }
                            }
                        }
                    }

                    result.Results.Add(UploadResult(file.FileName, file.Length, origionalFullPath, thumbsFullPath, resizedFullPath));
                }
                result.Succeeded = true;
                result.Message = "All files uploaded successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Succeeded = false;
                result.Message = "Something went worong while uploading files.";
            }
        }

        private MemoryStream SkiaSharpResizeImage(string localTempFilePath, int destWidthPx, int destHeightPx)
        {
            try
            {
                using var originalBmp = SKBitmap.Decode(localTempFilePath);
                using var scaledBmp = originalBmp.Resize(new SKImageInfo(destWidthPx, destHeightPx), SKFilterQuality.High);
                using var scaledImg = SKImage.FromBitmap(scaledBmp);

                SKEncodedImageFormat encodedImageFormat;
                int quality;

                var extension = Path.GetExtension(localTempFilePath).ToLower();
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        encodedImageFormat = SKEncodedImageFormat.Jpeg;
                        quality = 90;
                        break;

                    case ".png":
                        encodedImageFormat = SKEncodedImageFormat.Png;
                        quality = 100;
                        break;
                    default:
                        throw new NotSupportedException($"Unable to resize file: unsupported image type \"{extension}\"");
                }
                using var scaledImgData = scaledImg.Encode(encodedImageFormat, quality);
                var thumbnailStream = new MemoryStream();
                scaledImgData.SaveTo(thumbnailStream);
                thumbnailStream.Position = 0L;

                return thumbnailStream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled error trying to resize local image '{localTempFilePath}' to Width={destWidthPx}px, Height={destHeightPx}px", ex);
            }
        }

        private MemoryStream MagickNetResizeImage(string localTempFilePath, int destWidthPx, int destHeightPx)
        {
            try
            {
                // Read from file
                using (var collection = new MagickImageCollection(localTempFilePath))
                {
                    // This will remove the optimization and change the image to how it looks at that point
                    // during the animation. More info here: http://www.imagemagick.org/Usage/anim_basics/#coalesce
                    collection.Coalesce();

                    // Resize each image in the collection to a width of 200. When zero is specified for the height
                    // the height will be calculated with the aspect ratio.
                    foreach (MagickImage image in collection)
                    {
                        image.Resize(width: destWidthPx, height: destHeightPx);
                    }

                    // Save the result
                    var resizedImgStream = new MemoryStream();
                    collection.Write(resizedImgStream);
                    resizedImgStream.Position = 0L;

                    return resizedImgStream;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled error trying to resize local image '{localTempFilePath}' to Width={destWidthPx}px, Height={destHeightPx}px", ex);
            }
        }
        /// <summary>
        /// Looks at the file's extension and returns true if this is a GIF; false otherwise.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsGif(string extension) => ".gif".Equals(extension, StringComparison.OrdinalIgnoreCase);

        private void UploadPartialFile(FileUploadRequest request, string partialFileName)
        {
            throw new NotImplementedException();
        }

        private FileUploadResponse UploadResult(string fileName, long fileSizeInBytes, string fullPath, string thumbPath, string resizedPath)
        {
            var getType = MimeMapping.GetMimeMapping(fileName);

            var response = new FileUploadResponse()
            {
                name = fileName,
                size = fileSizeInBytes,
                type = getType,
                url = _filesHelper.UrlBase + fileName,
                origionalFilePath = fullPath,
                deleteUrl = _filesHelper.DeleteUrl + fileName,
                thumbnailUrl = _filesHelper.CheckThumb(getType, fileName),
                thumbnailFullPath = thumbPath,
                deleteType = _filesHelper.DeleteType,
            };

            return response;
        }
        #endregion
    }
}
