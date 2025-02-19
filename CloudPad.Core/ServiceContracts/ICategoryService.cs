using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.ServiceContracts;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync(int userId, Guid categoryId);
    Task<IEnumerable<CategoryDto>> GetAllAsync(int userId);
    Task<bool> ExistsAsync(int userId, Guid categoryId);
    Task<bool> ExistsAsync(int userId, string categoryName);

    Task<CategoryDto> CreateAsync(int userId, CreateCategoryDto category);
    Task<CategoryDto> UpdateAsync(int userId, UpdateCategoryDto category);

    Task<int?> FindCategoryIdByGuidAsync(int userId, Guid categoryId);

    Task<bool> DeleteAsync(int userId, Guid id);
    Task<CategoryDto?> GetByNameAsync(int userId, string categoryName);
}
