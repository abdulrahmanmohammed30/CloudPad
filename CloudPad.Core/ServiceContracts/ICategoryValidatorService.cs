namespace CloudPad.Core.ServiceContracts;

public interface ICategoryValidatorService
{
    Task<bool> ExistsAsync(int userId, Guid categoryId);
    Task<bool> ExistsAsync(int userId, string categoryName);
}