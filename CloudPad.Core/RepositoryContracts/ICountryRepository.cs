using CloudPad.Core.Entities;

namespace CloudPad.Core.RepositoryContracts;

public interface ICountryRepository
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<Country?> GetCountryByIdAsync(short id);
    Task<Country?> GetCountryByNameAsync(string name);
    Task<bool> ExistsAsync(short id);
}