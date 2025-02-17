using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.ServiceContracts
{
    public interface IResourceService
    {
        Task<ResourceDto> CreateResourceDto(int userId, string uploadsDirectoryPath, CreateResourceDto resourceDto);

        Task<List<ResourceDto>> GetAllResources(Guid noteId);
    }
}
