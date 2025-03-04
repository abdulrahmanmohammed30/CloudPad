using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface ITagRetrieverService
{
    Task<TagDto?> GetByIdAsync(int userId, int id);
    Task<IEnumerable<TagDto>> GetAllAsync(int userId);
    Task<TagDto?> GetByNameAsync(int userId, string name);
}