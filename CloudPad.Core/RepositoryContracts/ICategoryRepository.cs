using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.RepositoryContracts;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int userId, Guid categoryId);
    
    Task<List<Category>> GetAllAsync(int userId);
    
    Task<bool> ExistsAsync(int userId, Guid categoryId);
    Task<bool> ExistsAsync(int userId, string categoryName);

    Task<Category> CreateAsync(int userId, Category category);
    Task<Category> UpdateAsync(int userId, Category category);

    Task<int?> FindCategoryIdByGuidAsync(int userId, Guid categoryId);
}