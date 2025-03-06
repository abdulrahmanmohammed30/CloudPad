using CloudPad.Core.ServiceContracts;

namespace CloudPad.Core.Services;

public class UserValidatorService(IUserService userService) :IUserValidatorService
{
    public async Task EnsureUserValidationAsync(int userId)
    {
        //ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
    }
}

