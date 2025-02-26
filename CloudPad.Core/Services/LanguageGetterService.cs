using CloudPad.Core.Dtos;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.DTO;

namespace CloudPad.Core.Services;

public class LanguageGetterService(ILanguageRepository languageRepository, IMemoryCache cache) : ILanguageGetterService
{
    public async Task<List<LanguageDto>> GetAllAsync()
    {
        return await cache.GetOrCreateAsync("languages", async (entry) =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
            var languages = (await languageRepository.GetAllAsync()).Select(c => new LanguageDto()
            {
                Code = c.Code,
                Name = c.Name,
                LanguageId = c.LanguageId
            }).ToList();
            return languages;
        }) ?? new List<LanguageDto>();
    }
}