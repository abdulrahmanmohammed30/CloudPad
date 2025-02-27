﻿using CloudPad.Core.DTO;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

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
