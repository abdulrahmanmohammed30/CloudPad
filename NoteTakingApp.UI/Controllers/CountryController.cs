using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Controllers;

public class CountryController(IGetterCountryService getterCountryService) : Controller
{
    [HttpGet("/countries")]
    public async Task<IActionResult> GetAllCountries([FromQuery]short? id, [FromQuery]string? name)
    {
        if (id.HasValue)
        {
            var country= await getterCountryService.GetCountryById(id.Value);
            return country != null? Ok( country) : NotFound(new { message = "Country was not found." });
        }

        if (!string.IsNullOrEmpty(name))
        {
            var country= await getterCountryService.GetCountryByName(name);
            return country != null? Ok( country) : NotFound(new { message = "Country was not found." });
        }
        
        return Ok(await getterCountryService.GetAllCountries());
    }
}