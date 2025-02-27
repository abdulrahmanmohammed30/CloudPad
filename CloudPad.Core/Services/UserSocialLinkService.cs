using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;
using CloudPad.Core.Exceptions;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class UserSocialLinkService(IUserSocialLinkRepository userSocialLinkRepository):IUserSocialLinkService
{
    public async Task<UserSocialLinkDto> CreateAsync(CreateUserSocialLinkDto createUserSocialLinkDto)
    {
        var context = new ValidationContext(createUserSocialLinkDto);
        var errors = new List<ValidationResult>();

        if (Validator.TryValidateObject(createUserSocialLinkDto, context, errors, true) == false)
        {
            throw new InvalidateEmailRequestException(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid social link data");
        }

     
        var userSocialLink = new UserSocialLink()
        {
            UserId = createUserSocialLinkDto.UserId,
            Url = createUserSocialLinkDto.UserSocialLinkUrl,
        };
        return (await userSocialLinkRepository.CreateAsync(userSocialLink)).ToDto();
    }

    public async Task<bool> DeleteAsync(int userId, int socialLinkId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Invalid user id");
        }
        
        if (socialLinkId <= 0)
        {
            throw new ArgumentException("Invalid social Link Id");
        }
         return await userSocialLinkRepository.DeleteAsync(userId, socialLinkId);
    }
}