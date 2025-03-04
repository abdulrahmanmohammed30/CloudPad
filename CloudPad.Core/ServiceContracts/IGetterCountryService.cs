using CloudPad.Core.DTO;

namespace CloudPad.Core.ServiceContracts;

public interface IGetterCountryService
{
    Task<List<CountryDto>> GetAllCountriesAsync();
    Task<CountryDto?> GetCountryByIdAsync(short id);
    Task<CountryDto?> GetCountryByNameAsync(string name);
    Task<bool> ExistsAsync(short id);
}