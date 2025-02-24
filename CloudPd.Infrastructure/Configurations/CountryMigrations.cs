using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using NoteTakingApp.Core.Configurations;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Infrastructure.Configurations;

public class CountryMigrations(IOptions<List<CountryData>> countryDataOptions) : IEntityTypeConfiguration<Country>
{
    private List<CountryData> _countriesData => countryDataOptions.Value;

    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(GetCountries(_countriesData));
    }

    private List<Country> GetCountries(List<CountryData> countriesData)
    {
        short count = 1;
        List<Country> countries = new List<Country>();
        foreach (var country in countriesData)
        {
            countries.Add(new Country()
            {
                CountryId = count++,
                Alpha2Code = country.Alpha2Code,
                Alpha3Code = country.Alpha3Code,
                ArabicName = country.ArabicName,
                EnglishName = country.ENGLISHNAME,
              //  PhoneCode = country.PhoneCode
            });
        }
        return countries;
    }
}