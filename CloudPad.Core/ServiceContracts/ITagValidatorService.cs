namespace CloudPad.Core.ServiceContracts;

public interface ITagValidatorService
{
    Task<bool> ExistsAsync(int userId, int id);
    Task<bool> ExistsAsync(int userId, string name);
}