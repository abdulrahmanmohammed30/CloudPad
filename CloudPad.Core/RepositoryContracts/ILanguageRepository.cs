using CloudPad.Core.Entities;

namespace CloudPad.Core.RepositoryContracts;

public interface ILanguageRepository
{
    public Task<List<Language>> GetAllAsync();
}