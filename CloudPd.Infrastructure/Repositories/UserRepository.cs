using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities.Domains;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;

namespace NoteTakingApp.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<ApplicationUser?> GetUserByNameAsync(string userName)
    {
        var result = await context.Users
            .Include(user => user.Country)
            .Include(u => u.PreferredLanguage)
            .Include(u => u.SocialLinks)
            .Where(u => u.UserName == userName)
            .Join(context.UserRoles, u => u.Id, ur => ur.UserId, ((user, role) => new { user, role }))
            .Join(context.Roles, u => u.role.RoleId, r => r.Id, (user, role) => new { user, role })
            .Select(user => new
            {
                user,
                NotesCount = user.user.user.Notes.Count(),
                CategoriesCount = user.user.user.Categories.Count(),
                TagsCount = user.user.user.Tags.Count()
            })
            .FirstOrDefaultAsync();

        if (result == null) return null;

        var user = result.user.user.user;
        user.CategoriesCount = result.NotesCount;
        user.TagsCount = result.CategoriesCount;
        user.TagsCount = result.TagsCount;

        var role = result.user.role;
        user.Role = role;

        return user;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(int userId)
    {
        var result = await context.Users
            .Include(user => user.Country)
            .Include(u => u.PreferredLanguage)
            .Include(u => u.SocialLinks)
            .Where(u => u.Id == userId)
            .Join(context.UserRoles, u => u.Id, ur => ur.UserId, ((user, role) => new { user, role }))
            .Join(context.Roles, u => u.role.RoleId, r => r.Id, (user, role) => new { user, role })
            .Select(user => new
            {
                user,
                NotesCount = user.user.user.Notes.Count(),
                CategoriesCount = user.user.user.Categories.Count(),
                TagsCount = user.user.user.Tags.Count()
            })
            .FirstOrDefaultAsync();

        if (result == null) return null;

        var user = result.user.user.user;
        user.CategoriesCount = result.NotesCount;
        user.TagsCount = result.CategoriesCount;
        user.TagsCount = result.TagsCount;

        var role = result.user.role;
        user.Role = role;

        return user;
    }

    public async Task<List<ApplicationUserWithRole>> GetUsersAsync()
    {
        return await context.EnrichedUsers.ToListAsync();
    }

    public async Task<bool> ExistsAsync(int userId)
    {
        return await context.EnrichedUsers.AnyAsync(u => u.Id == userId);
    }
}