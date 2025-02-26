using CloudPad.Core.Entities;

namespace CloudPad.Core.RepositoryContracts;

public interface IUserSocialLinkRepository
{
    Task<UserSocialLink> CreateAsync(UserSocialLink userSocialLink);
    Task<bool> DeleteAsync(int userId, int socialLinkId);
}