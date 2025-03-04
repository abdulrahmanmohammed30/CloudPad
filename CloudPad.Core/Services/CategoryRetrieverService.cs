using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class CategoryRetrieverService(
    ICategoryRepository categoryRepository,
    IMemoryCache cache,
    IUserValidatorService userValidatorService) : ICategoryRetrieverService
{
    public async Task<CategoryDto?> GetByIdAsync(int userId, Guid categoryId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (categoryId == Guid.Empty)
            throw new InvalidCategoryIdException("Category id cannot be empty");

        var category = await categoryRepository.GetByIdAsync(userId, categoryId);
        return category?.ToDto();
    }

    public async Task<CategoryDto?> GetByNameAsync(int userId, string categoryName)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new InvalidCategoryException($"{categoryName} is not a valid category name");

        var category = await categoryRepository.GetByNameAsync(userId, categoryName);
        return category?.ToDto();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(int userId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        return await cache.GetOrCreateAsync($"Categories/{userId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            var categories = await categoryRepository.GetAllAsync(userId);
            return categories.ToDtoList();
        }) ?? throw new InvalidOperationException();
    }

    public async Task<int?> FindCategoryIdByGuidAsync(int userId, Guid categoryId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (categoryId == Guid.Empty)
            throw new InvalidCategoryIdException("Category id cannot be empty");

        return await categoryRepository.FindCategoryIdByGuidAsync(userId, categoryId);
    }
}