using Microsoft.AspNetCore.Http;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services
{
    public class ResourceService(INoteValidatorService noteValidatorService, IResourceRepository resourceRepository,
        IUserValidationService userValidationService) : IResourceService
    {
        private static readonly HashSet<string> fileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Excel
            ".xls", ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm", ".csv", ".ods",

            // PDF
            ".pdf",

            // Word
            ".doc", ".docx", ".dot", ".dotx", ".docm", ".dotm", ".odt",

            // Audio
            ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma", ".aiff", ".alac", ".opus"
        };

        private async Task<string> UploadDocument(string uploadsDirectoryPath, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required", nameof(file));
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

        public async Task<ResourceDto> CreateResourceDto(int userId, string uploadsDirectoryPath, CreateResourceDto resourceDto)
        {
            if (resourceDto == null) throw new ResourceArgumentNullException(nameof(resourceDto));

            if (resourceDto.DisplayName != null && resourceDto.DisplayName.Length > 255)
            {
                throw new InvalidResourceException("Display name is too long");
            }

            if (resourceDto.Description != null && resourceDto.Description.Length > 500)
            {
                throw new InvalidResourceException("Description is too long");
            }

            if (resourceDto.File == null)
            {
                throw new InvalidResourceException("File is required");
            }

            if (resourceDto.NoteId == Guid.Empty)
            {
                throw new InvalidResourceException("Note id is required");
            }

            // make sure noteid exists first 
            if (!await noteValidatorService.ExistsAsync(userId, resourceDto.NoteId))
            {
                throw new NoteNotFoundException($"Note with Id {resourceDto.NoteId} was not found");
            }

            var filepath = await UploadDocument(uploadsDirectoryPath, resourceDto.File);

            var resource = new Resource()
            {
                FilePath = filepath,
                DisplayName = resourceDto.DisplayName,
                Description = resourceDto.Description,
                Size = resourceDto.File.Length,
            };

            var createdResource = await resourceRepository.CreateAsync(resourceDto.NoteId, resource);

            return createdResource.ToDto();
        }

        public async Task<List<ResourceDto>> GetAllResources(Guid noteId)
        {
            return (await resourceRepository.GetAllAsync(noteId)).ToDtoList();
        }

        public async Task<bool> DeleteAsync(Guid resourceId)
        {
            if (resourceId == Guid.Empty)
            {
                throw new InvalidResourceIdException("Resource id is required");
            }

            var resource = await resourceRepository.Exists(resourceId);

            if (resource == false)
            {
                throw new ResourceNotFoundException($"Resource with Id {resourceId} was not found");
            }

            return await resourceRepository.DeleteAsync(resourceId);
        }
    }
}
