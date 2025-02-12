/*using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CloudPad.Core.Models;

namespace CloudPad.Core.Attributes.ValidationAttributes;

public class CountryValidationAttribute:ValidationAttribute
{
    private static List<Country> _countries = GetCountries();

    private static List<Country> GetCountries()
    {
        /*
        var config = new ConfigurationBuilder().AddJsonFile("countries.json").Build();
        _countries = config.GetSection("Countries").Get<List<Country>>();
        #1#
      
    }
    /*#1#
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
         
    }
}/*aa#1#*/