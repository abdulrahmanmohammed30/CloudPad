using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class UserSocialLinkService(IUserSocialLinkRepository userSocialLinkRepository):IUserSocialLinkService
{
    public async Task<UserSocialLinkDto> CreateAsync(CreateUserSocialLinkDto createUserSocialLinkDto)
    {
        if (createUserSocialLinkDto == null)
        {
            throw new ArgumentNullException(nameof(createUserSocialLinkDto));
        }

        if (createUserSocialLinkDto.UserId == 0)
        {
            throw new ArgumentException("User Id is required.", nameof(createUserSocialLinkDto.UserId));
        }
        
        if (string.IsNullOrEmpty(createUserSocialLinkDto.UserSocialLinkUrl))
        {
            throw new ArgumentException("Link Url is required.", nameof(createUserSocialLinkDto.UserSocialLinkUrl));
        }
        var userSocialLink = new UserSocialLink()
        {
            UserId = createUserSocialLinkDto.UserId,
            Url = createUserSocialLinkDto.UserSocialLinkUrl,
        };
        return (await userSocialLinkRepository.CreateAsync(userSocialLink)).ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
         return await userSocialLinkRepository.DeleteAsync(id);
    }
}