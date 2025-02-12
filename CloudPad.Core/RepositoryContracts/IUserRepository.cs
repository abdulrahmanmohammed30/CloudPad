using NoteTakingApp.Core.Domains;


namespace NoteTakingApp.Core.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<List<ApplicationUserWithRole>> GetEnrichedUsersAsync();
        Task<bool> ExistsAsync (int userId);
    }
}
