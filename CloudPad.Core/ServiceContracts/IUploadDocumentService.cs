using Microsoft.AspNetCore.Http;


namespace NoteTakingApp.Core.ServiceContracts
{
    public interface IUploadDocumentService
    {
        public Task<string> Upload(string uploadsDirectoryPath, IFormFile file);
    }
}
