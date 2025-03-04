using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface ICategoryRetrieverService
{
    Task<CategoryDto?> GetByIdAsync(int userId, Guid categoryId);
    Task<IEnumerable<CategoryDto>> GetAllAsync(int userId);
    Task<CategoryDto?> GetByNameAsync(int userId, string categoryName);
    Task<int?> FindCategoryIdByGuidAsync(int userId, Guid categoryId);
}