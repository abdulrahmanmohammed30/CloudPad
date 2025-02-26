using CloudPad.Core.Entities;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CloudPad.Infrastructure.Repositories;

public class LanguageRepository(AppDbContext context): ILanguageRepository
{
    public async Task<List<Language>> GetAllAsync()
    {
        return await context.Languages.ToListAsync();
    }
}