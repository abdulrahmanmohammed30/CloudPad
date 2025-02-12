using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;

namespace NoteTakingApp.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<List<ApplicationUserWithRole>> GetEnrichedUsersAsync()
    {
        return await context.EnrichedUsers.ToListAsync();
    }

    public async Task<bool> ExistsAsync(int userId)
    {
        return await context.EnrichedUsers.AnyAsync(u => u.Id == userId);
    }
}
