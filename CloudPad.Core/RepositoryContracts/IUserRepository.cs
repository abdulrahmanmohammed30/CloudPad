using CloudPad.Core.Domains;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.Dtos;


namespace CloudPad.Core.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<List<ApplicationUserWithRole>> GetUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
        Task<ApplicationUser?> GetUserByNameAsync(string username);
        Task<bool> ExistsAsync (int userId);
        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task DeleteUserAsync(int userId);

    }
}
