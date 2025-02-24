using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.RepositoryContracts;

public interface ILanguageRepository
{
    public Task<List<Language>> GetAllAsync();
}