using CloudPad.Core.DTO;

namespace CloudPad.Core.ServiceContracts;

public interface IGetterCountryService
{
    Task<List<CountryDto>> GetAllCountries();
    Task<CountryDto?> GetCountryById(short id);
    Task<CountryDto?> GetCountryByName(string name);
    Task<bool> Exists(short id);
}