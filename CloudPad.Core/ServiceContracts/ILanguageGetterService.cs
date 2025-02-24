using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.ServiceContracts;

public interface ILanguageGetterService
{
    Task<List<LanguageDto>> GetAllAsync();
}