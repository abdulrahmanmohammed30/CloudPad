using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface ICategoryManagerService
{
    Task<CategoryDto> CreateAsync(int userId, CreateCategoryDto category);
    Task<CategoryDto> UpdateAsync(int userId, UpdateCategoryDto category);
    Task<bool> DeleteAsync(int userId, Guid id);
}