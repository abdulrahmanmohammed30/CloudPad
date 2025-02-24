using NoteTakingApp.Core.Dtos;


namespace NoteTakingApp.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync();
        Task<bool> ExistsAsync(int userId);
        Task<ProfileDto?> GetUserByIdAsync(int userId);
        Task<ProfileDto?> GetUserByNameAsync(string username);

        Task<ProfileDto> UpdateProfileAsync(UpdateProfileDto profile);
        
        Task DeleteUserAsync(int userId);
    }
}