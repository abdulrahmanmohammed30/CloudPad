using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface IUserSocialLinkService
{
    Task<UserSocialLinkDto> CreateAsync(CreateUserSocialLinkDto createUserSocialLinkDto);
    Task<bool> DeleteAsync(int userId, int socialLinkId);
}