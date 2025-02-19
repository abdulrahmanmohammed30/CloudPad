using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class UserValidationService(IUserService userService) :IUserValidationService
{
    public async Task EnsureUserValidation(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
    }
}

