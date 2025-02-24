using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;

namespace NoteTakingApp.Infrastructure.Repositories;

public class UserSocialLinkRepository(AppDbContext context):IUserSocialLinkRepository
{
    public async Task<UserSocialLink> CreateAsync(UserSocialLink userSocialLink)
    {
        context.UserSocialLinks.Add(userSocialLink);
        await context.SaveChangesAsync();
        return userSocialLink;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rows = await context.UserSocialLinks
            .Where(l=>l.UserSocialLinkId == id)
            .ExecuteDeleteAsync();
        return rows > 0;
    }
}