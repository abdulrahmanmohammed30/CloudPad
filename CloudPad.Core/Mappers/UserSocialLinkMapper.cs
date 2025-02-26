using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;

namespace CloudPad.Core.Mappers;

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