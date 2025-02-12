using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class CategoryService(ICategoryRepository categoryRepository,   
    IUserService userService
) : ICategoryService
{
    private async Task ValidateUserAsync(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

        if (!await userService.ExistsAsync(userId))
        {
            throw new UserNotFoundException($"User with id {userId} doesn't exist");
        }
    }
    
    public async Task<CategoryDto?> GetByIdAsync(int userId, Guid categoryId)
    {
        await ValidateUserAsync(userId);
        
        var category = await categoryRepository.GetByIdAsync(userId, categoryId);
        return category?.ToDto();
    }
    
    public async Task<bool> ExistsAsync(int userId, string categoryName)
    {
        await ValidateUserAsync(userId);
        
        return await categoryRepository.ExistsAsync(userId, categoryName);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(int userId)
    {
        await ValidateUserAsync(userId);

        var categories = await categoryRepository.GetAllAsync(userId);
        return categories.ToDtoList();
    }

    public async Task<bool> ExistsAsync(int userId, Guid categoryId)
    {
        return await categoryRepository.ExistsAsync(userId, categoryId);
    }

    public async Task<CategoryDto> CreateAsync(int userId, CreateCategoryDto categoryDto)
    {        
        await ValidateUserAsync(userId);

        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new ArgumentNullException(nameof(categoryDto.Name), "Category name cannot be null or empty");
        }

        if (await ExistsAsync(userId, categoryDto.Name))
        {
            throw new DuplicateCategoryNameException("Category name already exists");
        }
        
        var category = categoryDto.ToEntity();
        var createdCategory = await categoryRepository.CreateAsync(userId, category);
        return createdCategory.ToDto();
    }

    public async Task<CategoryDto> UpdateAsync(int userId, UpdateCategoryDto categoryDto)
    {
        await ValidateUserAsync(userId);
        
        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new ArgumentNullException(nameof(categoryDto.Name), "Category name cannot be null or empty");
        }

        if (await ExistsAsync(userId, categoryDto.Name))
        {
            throw new DuplicateCategoryNameException("Category name already exists");
        }
        
        var category = categoryDto.ToEntity();
        var updatedCategory = await categoryRepository.UpdateAsync(userId, category);
        return updatedCategory.ToDto();
    }

    public async Task<int?> FindCategoryIdByGuidAsync(int userId, Guid categoryId)
    {
        return await categoryRepository.FindCategoryIdByGuidAsync(userId, categoryId);
    }
}