using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPad.Core.ServiceContracts
{
    public interface IUploadImageService
    {
        public Task<string> Upload(string uploadsDirectoryPath, IFormFile file);
    }
}
