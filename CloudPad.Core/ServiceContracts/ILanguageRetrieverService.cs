using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface ILanguageRetrieverService
{
    Task<List<LanguageDto>> GetAllAsync();
}