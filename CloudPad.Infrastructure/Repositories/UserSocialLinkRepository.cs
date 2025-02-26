using CloudPad.Core.Entities;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CloudPad.Infrastructure.Repositories;

public class UserSocialLinkRepository(AppDbContext context):IUserSocialLinkRepository
{
    public async Task<UserSocialLink> CreateAsync(UserSocialLink userSocialLink)
    {
        context.UserSocialLinks.Add(userSocialLink);
        await context.SaveChangesAsync();
        return userSocialLink;
    }

    public async Task<bool> DeleteAsync(int userId, int socialLinkId)
    {
        var rows = await context.UserSocialLinks
            .Where( l=>l.UserId == userId && l.UserSocialLinkId == socialLinkId)
            .ExecuteDeleteAsync();
        return rows > 0;
    }
}