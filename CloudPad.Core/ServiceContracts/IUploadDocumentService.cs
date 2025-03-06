using Microsoft.AspNetCore.Http;

namespace CloudPad.Core.ServiceContracts
{
    public interface IUploadDocumentService
    {
        public Task<string> UploadAsync(string uploadsDirectoryPath, IFormFile file);
    }
}
