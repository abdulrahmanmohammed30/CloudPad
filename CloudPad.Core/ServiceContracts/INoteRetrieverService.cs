using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface INoteRetrieverService
{
    Task<NoteDto?> GetByIdAsync(int userId, Guid noteId);
    Task<IEnumerable<NoteDto>> GetAllAsync(int userId, int pageNumber = 0, int pageSize = 20);
}