using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.RepositoryContracts;

public interface IUserSocialLinkRepository
{
    Task<UserSocialLink> CreateAsync(UserSocialLink userSocialLink);
    Task<bool> DeleteAsync(int id);
}