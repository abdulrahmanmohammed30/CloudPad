using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;
namespace NoteTakingApp.Infrastructure.Repositories;


public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(int userId, Guid categoryId)
    {
        return await context.Categories
            .Where(c => c.UserId == userId && c.CategoryGuid == categoryId)
            .FirstOrDefaultAsync();
    }
    

    public async Task<List<Category>> GetAllAsync(int userId)
    {
        return await context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Category>> GetFavoritesAsync(int userId)
    {
        return await context.Categories
            .Where(c => c.UserId == userId && c.IsFavorite)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int userId, string categoryName)
    {
        return await context.Categories
            .AnyAsync(c => c.UserId == userId && c.Name == categoryName);
    }

    public async Task<Category> CreateAsync(int userId, Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        
        return category;
    }

    public async Task<Category> UpdateAsync(int userId, Category category)
    {
        context.Update(category);
        await context.SaveChangesAsync();
        
        return category;
    }

    public async Task<int?> FindCategoryIdByGuidAsync(int userId, Guid categoryId)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryGuid == categoryId);
        return category?.CategoryId;
    }

    public async Task<bool> DeleteAsync(int userId, Guid categoryGuid)
    {
        var existingCategory = await GetByIdAsync(userId, categoryGuid);
        existingCategory.IsDeleted = true;
        context.Update(existingCategory);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int userId, Guid categoryGuid)
    {
        return await context.Categories
            .AnyAsync(c => c.UserId == userId && c.CategoryGuid == categoryGuid);
    }

    public async Task<Category?> GetByNameAsync(int userId, string categoryName)
    {
        return await context.Categories
             .FirstOrDefaultAsync(c => c.UserId == userId && c.Name == categoryName);
    }
}
