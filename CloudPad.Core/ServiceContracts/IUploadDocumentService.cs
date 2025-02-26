using Microsoft.AspNetCore.Http;


namespace CloudPad.Core.ServiceContracts
{
    public interface IUploadDocumentService
    {
        public Task<string> Upload(string uploadsDirectoryPath, IFormFile file);
    }
}
