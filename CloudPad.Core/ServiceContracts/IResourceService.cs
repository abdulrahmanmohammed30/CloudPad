using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.ServiceContracts
{
    public interface IResourceService
    {
        Task<ResourceDto> CreateResourceDto(int userId, string uploadsDirectoryPath, CreateResourceDto resourceDto);

        Task<List<ResourceDto>> GetAllResources(Guid resourceId);

        Task<ResourceDto> UpdateAsync(int userId, UpdateResourceDto resource);

        Task<bool> DeleteAsync(Guid resourceId);

        Task<ResourceDto?> GetByIdAsync(Guid resourceId);
    }
}
