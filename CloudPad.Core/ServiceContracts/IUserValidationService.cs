namespace NoteTakingApp.Core.ServiceContracts;

public interface IUserValidationService
{
    Task EnsureUserValidation(int userId);
}