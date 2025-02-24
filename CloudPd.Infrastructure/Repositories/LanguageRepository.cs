using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;

namespace NoteTakingApp.Infrastructure.Repositories;

public class LanguageRepository(AppDbContext context): ILanguageRepository
{
    public async Task<List<Language>> GetAllAsync()
    {
        return await context.Languages.ToListAsync();
    }
}