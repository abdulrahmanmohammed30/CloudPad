using CloudPad.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;


namespace CloudPad.Core.Services
{
    public class UploadImageService(IUploadDocumentService uploadDocumentService) : IUploadImageService
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        public Task<string> Upload(string uploadsDirectoryPath, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required", nameof(file));
            }

            if (string.IsNullOrWhiteSpace(uploadsDirectoryPath))
            {
                throw new ArgumentNullException(nameof(uploadsDirectoryPath), "uploadsDirectoryPath cannot be null or empty");
            }


            var extension = Path.GetExtension(file.FileName);

            if (extension == null)
            {
                throw new ArgumentException("Invalid file type", nameof(file));

            }
            if (_allowedExtensions.Contains(extension.ToLower()) == false)
            {
                throw new InvalidOperationException("Only image files are allowed");
            }

            return uploadDocumentService.Upload(uploadsDirectoryPath, file);
        }
    }
}
