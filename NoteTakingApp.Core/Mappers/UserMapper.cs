using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.Mappers;

public static class UserMapper
{
    public static ApplicationUser ToEntity(this RegisterDto registerDto)
    {
        return new ApplicationUser()
        {
            BirthDate = registerDto.BirthDate, 
            CountryId = (short?)registerDto.CountryId,
            Name = registerDto.Name.Trim(),
            ProfileImageUrl = registerDto.ProfileImageUrl?.Trim(),
            UserName = registerDto.UserName.Trim()
        };
    }
}