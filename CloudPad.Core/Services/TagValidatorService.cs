using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;

namespace NoteTakingApp.Core.Services;

public class TagValidatorService(
    ITagRepository tagRepository,
    IUserValidatorService userValidatorService) : ITagValidatorService
{
    public async Task<bool> ExistsAsync(int userId, int id)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        return await tagRepository.ExistsAsync(userId, id);
    }

    public async Task<bool> ExistsAsync(int userId, string name)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        return await tagRepository.ExistsAsync(userId, name);
    }
}