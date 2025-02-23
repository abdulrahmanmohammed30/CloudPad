using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities.Domains;


namespace NoteTakingApp.Core.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<List<ApplicationUserWithRole>> GetUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
        Task<ApplicationUser?> GetUserByNameAsync(string username);
        Task<bool> ExistsAsync (int userId);
    }
}
