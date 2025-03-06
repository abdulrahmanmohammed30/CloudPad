using CloudPad.Core.Dtos;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;

namespace NoteTakingApp.Core.Services;

public class TagRetrieverService(ITagRepository tagRepository, IUserValidatorService userValidatorService, IMemoryCache cache) : ITagRetrieverService
{
    public async Task<TagDto?> GetByIdAsync(int userId, int id)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        var tag = await tagRepository.GetByIdAsync(userId, id);
        return tag?.ToDto();
    }

    public async Task<IEnumerable<TagDto>> GetAllAsync(int userId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        return await cache.GetOrCreateAsync($"tags/{userId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            await userValidatorService.EnsureUserValidationAsync(userId);
            var tags = await tagRepository.GetAllAsync(userId);
            return tags.ToDtoList();
        }) ?? throw new InvalidOperationException();
    }

    public async Task<TagDto?> GetByNameAsync(int userId, string name)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        var tag = await tagRepository.GetByNameAsync(userId, name);
        return tag?.ToDto();
    }
}
