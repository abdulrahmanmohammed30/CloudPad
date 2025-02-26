using CloudPad.Core.Domains;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using CloudPad.Core.Dtos;

namespace CloudPad.Infrastructure.Repositories;

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

    public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
    {
        context.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUserAsync(int userId)
    {
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u=>u.Id == userId);
                if (user == null)
                {
                    throw new UserNotFoundException();
                }

                context.Tags.RemoveRange(context.Tags.Where(t => t.UserId == userId));
                await context.SaveChangesAsync();
                
                context.Remove(user);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
               await transaction.RollbackAsync();
               throw;
            }
        }
    }
}