using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;

namespace CloudPad.Core.ServiceContracts
{
    public interface IResourceService
    {
        Task<ResourceDto> CreateResourceDto(int userId, string uploadsDirectoryPath, CreateResourceDto resourceDto);

        Task<List<ResourceDto>> GetAllResources(int userId, Guid resourceId);

        Task<ResourceDto> UpdateAsync(int userId, Guid noteId, UpdateResourceDto resource);

        Task DeleteAsync(int userId, Guid noteId, Guid resourceId);

        Task<ResourceDto?> GetByIdAsync(int userId, Guid noteId, Guid resourceId);
    }
}
