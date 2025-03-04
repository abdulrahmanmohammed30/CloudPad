using Microsoft.AspNetCore.Http;

namespace CloudPad.Core.ServiceContracts
{
    public interface IUploadImageService
    {
        public Task<string> UploadAsync(string uploadsDirectoryPath, IFormFile file);
    }
}
