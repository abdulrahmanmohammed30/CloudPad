using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.Mappers;

public static class UserSocialLinkMapper
{
    public static UserSocialLinkDto ToDto(this UserSocialLink userSocialLink)
    {
        return new UserSocialLinkDto()
        {
            UserSocialLinkId = userSocialLink.UserSocialLinkId,
            UserSocialLinkUrl = userSocialLink.Url
        };
    }
}