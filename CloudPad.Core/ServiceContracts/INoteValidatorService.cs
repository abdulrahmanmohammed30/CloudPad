namespace NoteTakingApp.Core.ServiceContracts;

public interface INoteValidatorService
{
    Task<bool> ExistsAsync(int userId, Guid noteId);
}