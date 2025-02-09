using NoteTakingApp.Core.DTO;
using NoteTakingApp.Core.Models;

namespace NoteTakingApp.Core.Mappers;

public static class CountryMapper
{
    public static CountryDto ToDto(this Country country)
    {
        return new CountryDto
        {
            Id = country.CountryId,
            Name = country.EnglishName,
            Translation = country.ArabicName
        };
    }
}