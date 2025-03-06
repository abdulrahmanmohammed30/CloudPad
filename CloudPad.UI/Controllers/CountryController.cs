using CloudPad.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPad.Controllers;

public class CountryController(ICountryRetrieverService countryRetrieverService) : Controller
{
    [HttpGet("/countries")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCountries([FromQuery]short? id, [FromQuery]string? name)
    {
        if (id.HasValue)
        {
            var country= await countryRetrieverService.GetCountryByIdAsync(id.Value);
            return country != null? Ok( country) : NotFound(new { message = "Country was not found." });
        }

        if (!string.IsNullOrEmpty(name))
        {
            var country= await countryRetrieverService.GetCountryByNameAsync(name);
            return country != null? Ok( country) : NotFound(new { message = "Country was not found." });
        }
        
        return Ok(await countryRetrieverService.GetAllCountriesAsync());
    }
}