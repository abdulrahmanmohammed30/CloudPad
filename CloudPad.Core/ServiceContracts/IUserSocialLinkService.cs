using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.ServiceContracts;

public interface IUserSocialLinkService
{
    Task<UserSocialLinkDto> CreateAsync(CreateUserSocialLinkDto createUserSocialLinkDto);
    Task<bool> DeleteAsync(int id);
}