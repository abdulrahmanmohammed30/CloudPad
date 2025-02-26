using CloudPad.Core.ServiceContracts;

namespace CloudPad.Core.Services;

public class UserValidationService(IUserService userService) :IUserValidationService
{
    public async Task EnsureUserValidation(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
    }
}

