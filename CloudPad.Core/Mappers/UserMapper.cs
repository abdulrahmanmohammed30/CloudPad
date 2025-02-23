using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities.Domains;

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
            UserName = registerDto.UserName.Trim(),
            Email=registerDto.Email.Trim()
        };
    }

    public static UserDto ToDto(this ApplicationUserWithRole user)
    {
        return
            new UserDto()
            {
                Id = user.Id,
                Username = user?.UserName,
                CountryId = user?.CountryId,
                BirthDate = user?.BirthDate,
                Name = user.Name,
                ProfileImageUrl = user.ProfileImageUrl?.Trim(),
                CountryName=user.CountryName,
                Role=string.IsNullOrEmpty(user.RoleName) == true? "User": user.RoleName,
                Email = user.Email,
            };
    }
}