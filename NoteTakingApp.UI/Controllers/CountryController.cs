using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Controllers;

public class CountryController : Controller
{
    private readonly IGetterCountryService _getterCountryService;

    public CountryController(IGetterCountryService getterCountryService)
    {
        _getterCountryService = getterCountryService;
    }
    
    [HttpGet("/countries")]
    public async Task<IActionResult> GetAllCountries([FromQuery]short? id, [FromQuery]string? name)
    {
        if (id.HasValue)
        {
            var country= await _getterCountryService.GetCountryById(id.Value);
            return country != null? Ok( country) : NotFound(new { message = "Country was not found." });
        }

        if (!string.IsNullOrEmpty(name))
        {
            var country= await _getterCountryService.GetCountryByName(name);
            return country != null? Ok( country) : NotFound(new { message = "Country was not found." });
        }
        
        return Ok(await _getterCountryService.GetAllCountries());
    }
}