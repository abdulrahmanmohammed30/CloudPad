using NoteTakingApp.Core.Dtos;


namespace NoteTakingApp.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync();
        Task<bool> ExistsAsync (int userId);
    }
}