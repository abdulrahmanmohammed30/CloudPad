using Microsoft.AspNetCore.Http;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services
{
    public class ResourceService(INoteRepository noteRepository, IResourceRepository resourceRepository,
       IUploadDocumentService  uploadDocumentService ) : IResourceService
    {

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
            var note = await noteRepository.GetById(userId, resourceDto.NoteId);
            if (note == null)
            {
                throw new NoteNotFoundException($"Note with Id {resourceDto.NoteId} was not found");
            }

            var filepath = await uploadDocumentService.Upload(uploadsDirectoryPath, resourceDto.File);

            var resource = new Resource()
            {
                FilePath = filepath,
                DisplayName = resourceDto.DisplayName,
                Description = resourceDto.Description,
                Size = resourceDto.File.Length,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                NoteId = note.NoteId, 
                Note = note
        };

            var createdResource = await resourceRepository.CreateAsync(resource);

            return createdResource.ToDto();
        }

        public async Task<List<ResourceDto>> GetAllResources(int userId, Guid noteId)
        {
            return (await resourceRepository.GetAllAsync(userId, noteId)).ToDtoList();
        }

        public async Task DeleteAsync(int userId, Guid noteId, Guid resourceId)
        {
            if (resourceId == Guid.Empty)
            {
                throw new InvalidResourceIdException("Resource id is required");
            }

            var resource = await resourceRepository.GetByIdAsync(userId, noteId, resourceId);

            if (resource == null)
            {
                throw new ResourceNotFoundException($"Resource with Id {resourceId} was not found");
            }

            resource.IsDeleted = true;
            await resourceRepository.UpdateAsync(resource);
        }

        public async Task<ResourceDto> UpdateAsync(int userId, Guid noteId, UpdateResourceDto resourceDto)
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

            if (resourceDto.NoteId == Guid.Empty)
            {
                throw new InvalidResourceException("Note id is required");
            }

            // make sure noteid exists first 
            if (!await noteRepository.ExistsAsync(userId, resourceDto.NoteId))
            {
                throw new NoteNotFoundException($"Note with Id {resourceDto.NoteId} was not found");
            }

            var resource = await resourceRepository.GetByIdAsync(userId, noteId, resourceDto.ResourceId);

            if(resource == null)
            {
                throw new ResourceNotFoundException($"Resource with Id {resourceDto.ResourceId} was not found");
            }

            resource.DisplayName = resourceDto.DisplayName;
            resource.Description = resourceDto.Description;
            resource.UpdatedAt = DateTime.UtcNow;

            var updatedResource = await resourceRepository.UpdateAsync(resource);

            return updatedResource.ToDto();
        }

        public async Task<ResourceDto?> GetByIdAsync(int userId, Guid noteId, Guid resourceId)
        {
            return (await resourceRepository.GetByIdAsync(userId, noteId, resourceId))?.ToDto();
        }
    }
}
