using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class CategoryManagerService(
    ICategoryRepository categoryRepository,
    IMemoryCache cache,
    IUserValidatorService userValidatorService,
    ICategoryValidatorService categoryValidatorService) : ICategoryManagerService
{
    public async Task<CategoryDto> CreateAsync(int userId, CreateCategoryDto categoryDto)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (categoryDto == null)
        {
            throw new CategoryArgumentNullException($"category was null");
        }

        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new InvalidCategoryException("Category name cannot be null or empty");
        }

        if (await categoryValidatorService.ExistsAsync(userId, categoryDto.Name))
        {
            throw new DuplicateCategoryNameException("Category name already exists");
        }

        if (categoryDto.Description != null && categoryDto.Description.Length > 500)
        {
            throw new InvalidCategoryException("Category description cannot be more than 500 characters");
        }

        var category = categoryDto.ToEntity();
        category.UserId = userId;
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;
        category.CategoryGuid = Guid.NewGuid();

        var createdCategory = await categoryRepository.CreateAsync(userId, category);

        cache.Remove($"Categories/{userId}");

        return createdCategory.ToDto();
    }

    public async Task<CategoryDto> UpdateAsync(int userId, UpdateCategoryDto categoryDto)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (categoryDto == null)
        {
            throw new CategoryArgumentNullException($"category was null");
        }

        if (categoryDto.CategoryId == Guid.Empty)
        {
            throw new InvalidCategoryIdException("Category id cannot be empty");
        }

        if (string.IsNullOrEmpty(categoryDto.Name))
        {
            throw new InvalidCategoryException("Category name cannot be null or empty");
        }

        if (categoryDto.Description != null && categoryDto.Description.Length > 500)
        {
            throw new InvalidCategoryException("Category description cannot be more than 500 characters");
        }

        var category = await categoryRepository.GetByIdAsync(userId, categoryDto.CategoryId);

        if (category == null)
        {
            throw new CategoryNotFoundException("Category was not found");
        }

        if (category.Name != categoryDto.Name && await categoryValidatorService.ExistsAsync(userId, categoryDto.Name))
        {
            throw new DuplicateCategoryNameException("Category name already exists");
        }

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.IsFavorite = categoryDto.IsFavorite;
        category.UpdatedAt = DateTime.UtcNow;

        var updatedCategory = await categoryRepository.UpdateAsync(userId, category);

        cache.Remove($"Categories/{userId}");

        return updatedCategory.ToDto();
    }

    public async Task<bool> DeleteAsync(int userId, Guid categoryId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (categoryId == Guid.Empty)
            throw new InvalidCategoryIdException("Category id cannot be empty");

        var category = await categoryRepository.GetByIdAsync(userId, categoryId);

        if (category == null)
        {
            throw new CategoryNotFoundException($"Category with id {categoryId} was not found");
        }

        category.IsDeleted = true;

        await categoryRepository.UpdateAsync(userId, category);

        cache.Remove($"Categories/{userId}");
        return true;
    }
}