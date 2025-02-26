using CloudPad.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;


namespace CloudPad.Core.Services
{
    public class UploadImageService(IUploadDocumentService uploadDocumentService) : IUploadImageService
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        public Task<string> Upload(string uploadsDirectoryPath, IFormFile file)
        {
            if (uploadsDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(uploadsDirectoryPath));
            }

            if  (file == null)
            {
                throw new ArgumentNullException(nameof(file));
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
