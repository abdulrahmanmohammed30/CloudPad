using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class UserValidationService(IUserService userService):IUserValidationService
{
    public async Task EnsureUserValidation(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

        // Todo, read userId from the HttpContext
        //if (!await userService.ExistsAsync(userId))
        //{
        //    throw new UserNotFoundException($"User with id {userId} doesn't exist");
        //}
    }
}