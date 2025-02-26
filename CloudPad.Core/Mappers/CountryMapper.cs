using CloudPad.Core.DTO;
using CloudPad.Core.Entities;

namespace CloudPad.Core.Mappers;

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