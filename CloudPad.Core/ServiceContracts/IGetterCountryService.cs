using NoteTakingApp.Core.DTO;

namespace NoteTakingApp.Core.ServiceContracts;

public interface IGetterCountryService
{
    Task<List<CountryDto>> GetAllCountries();
    Task<CountryDto?> GetCountryById(short id);
    Task<CountryDto?> GetCountryByName(string name);
    Task<bool> Exists(short id);
}