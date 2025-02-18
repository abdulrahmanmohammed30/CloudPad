using Microsoft.EntityFrameworkCore;
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
        category.UserId = userId;
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;
        category.CategoryGuid = Guid.NewGuid();
        
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        
        return category;
    }

    public async Task<Category> UpdateAsync(int userId, Category category)
    {
        var existingCategory = await context.Categories.AsTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId && c.CategoryGuid == category.CategoryGuid);

        if (existingCategory == null)
            throw new CategoryNotFoundException($"Category with GUID {category.CategoryGuid} not found");

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.IsFavorite = category.IsFavorite;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        context.Update(existingCategory);
        await context.SaveChangesAsync();
        
        return existingCategory;
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
}
