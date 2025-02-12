using Microsoft.Extensions.Caching.Memory;
using NoteTakingApp.Core.DTO;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class GetterCountryService : IGetterCountryService
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "CountriesCache";

    public GetterCountryService(ICountryRepository countryRepository, IMemoryCache cache)
    {
        _countryRepository = countryRepository;
        _cache = cache;
    }

    public async Task<List<CountryDto>> GetAllCountries()
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            var countries = await _countryRepository.GetAllCountriesAsync();
            return countries.Select(c => c.ToDto()).ToList();
        }) ?? throw new InvalidOperationException();
        
        // if (_cache.GetOrCreateAsync())
        // {
        //     
        // }   
        // if (!_cache.TryGetValue(CacheKey, out List<CountryDTO>? countries))
        // {
        //     countries = (await _countryRepository.GetAllCountriesAsync()).Select(c=>c.ToDto());
        //     _cache.Set(CacheKey, countries, TimeSpan.FromDays(1));
        // }
        // return countries;  
    }

    public async Task<CountryDto?> GetCountryById(short id)
    {
        var countries = await GetAllCountries(); 
        return  countries.FirstOrDefault(c => c.Id == id);
    }

    public async Task<CountryDto?> GetCountryByName(string name)
    {
        var countries = await GetAllCountries(); 
        return countries.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> Exists(short id)
    {
        var countries = await GetAllCountries(); 
        return countries.Any(c => c.Id == id);
    }
}
