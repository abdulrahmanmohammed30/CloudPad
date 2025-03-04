using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class CategoryValidatorService(
    ICategoryRepository categoryRepository,
    IUserValidatorService userValidatorService) : ICategoryValidatorService
{
    public async Task<bool> ExistsAsync(int userId, string categoryName)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new InvalidCategoryException($"{categoryName} is not a valid category name");

        return await categoryRepository.ExistsAsync(userId, categoryName);
    }

    public async Task<bool> ExistsAsync(int userId, Guid categoryId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (categoryId == Guid.Empty)
            throw new InvalidCategoryIdException("Category id cannot be empty");

        return await categoryRepository.ExistsAsync(userId, categoryId);
    }
}