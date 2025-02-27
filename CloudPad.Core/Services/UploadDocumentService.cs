using CloudPad.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;


namespace CloudPad.Core.Services
{
    public class UploadDocumentService: IUploadDocumentService
    {
        private static readonly HashSet<string> fileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Image 
             ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".webp", ".ico", ".tiff", ".tif", ".psd", ".raw", ".heif", ".indd", ".ai", ".eps", ".pdf", ".jfif", ".jpe", ".jif", ".jfi", ".jp2", ".j2k", ".jpf", ".jpx", ".jpm", ".mj2", ".jxr", ".hdp", ".wdp", ".cur", ".dds", ".dng", ".cr2", ".nef", ".nrw", ".arw", ".pef",
            ".raf", ".orf", ".rw2", ".rwl", ".srw", ".sr2", ".srf", ".x3f", ".erf", ".mrw", ".mef", ".mos ", ".kdc", ".dcr", ".ptx", ".pxn", ".r3d", ".raf", ".3fr", ".qtk", ".rwz", ".rwl", ".rwz", ".rw2",
            // Excel
            ".xls", ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm", ".csv", ".ods",

            // PDF
            ".pdf",

            // Word
            ".doc", ".docx", ".dot", ".dotx", ".docm", ".dotm", ".odt",

            // Audio
            ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma", ".aiff", ".alac", ".opus"
        };

        public async Task<string> Upload(string uploadsDirectoryPath, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required", nameof(file));
            }

            if (string.IsNullOrWhiteSpace(uploadsDirectoryPath))
            {
                throw new ArgumentNullException(nameof(uploadsDirectoryPath), "uploadsDirectoryPath cannot be null or empty");
            }

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);

            if (extension == null)
            {
                throw new ArgumentException("Invalid file type", nameof(file));
            }

            if (!fileExtensions.Contains(extension))
            {
                throw new ArgumentException("File type is not supported", nameof(file));
            }

            var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + extension;

            var filepath = Path.Combine(uploadsDirectoryPath, uniqueFileName);

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var uploadsDirectory = Path.GetDirectoryName(filepath).Split(Path.DirectorySeparatorChar).LastOrDefault();

            return Path.Combine(uploadsDirectory, uniqueFileName);
        }

    }
}
