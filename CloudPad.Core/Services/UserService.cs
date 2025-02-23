using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<UserDto>> GetUsersAsync()
        {
            return (await userRepository.GetUsersAsync()).Select(c => c.ToDto()).OrderBy(c => c.Id).ToList();
        }

        public async Task<ProfileDto> GetUserByIdAsync(int userId)
        {
            return (await userRepository.GetUserByIdAsync(userId)).ToProfileDto();
        }

        public async Task<ProfileDto> GetUserByNameAsync(string username)
        {
            return (await userRepository.GetUserByNameAsync(username)).ToProfileDto();
        }

        public Task<bool> ExistsAsync(int userId)
        {
            return userRepository.ExistsAsync(userId);
        }
    }
}