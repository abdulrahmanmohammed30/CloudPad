using CloudPad.Core.DTO;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class GetterCountryService(ICountryRepository countryRepository, IMemoryCache cache) : IGetterCountryService
{
    private const string CacheKey = "CountriesCache";

    public async Task<List<CountryDto>> GetAllCountries()
    {
        return await cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            var countries = await countryRepository.GetAllCountriesAsync();
            return countries.Select(c => c.ToDto()).ToList();
        }) ?? throw new InvalidOperationException();
    }

    public async Task<CountryDto?> GetCountryById(short id)
    {
        if (id <= 0)
        {
            throw new InvalidCategoryException("id cannot be less than or equal 0");
        } 
        
        return await countryRepository.GetCountryByIdAsync(id).ContinueWith(c => c.Result?.ToDto()); 
    }

    public async Task<CountryDto?> GetCountryByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidCategoryException("name cannot be null or empty");
        }

        return await countryRepository.GetCountryByNameAsync(name).ContinueWith(c => c.Result?.ToDto()); 
    }

    public async Task<bool> Exists(short id)
    {
        if (id <= 0)
        {
            return false;
        }

       return await countryRepository.ExistsAsync(id);
    }
}
