namespace CloudPad.Core.ServiceContracts;

public interface IUserValidatorService
{
    Task EnsureUserValidationAsync(int userId);
}