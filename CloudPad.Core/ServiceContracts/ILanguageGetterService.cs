using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface ILanguageGetterService
{
    Task<List<LanguageDto>> GetAllAsync();
}