using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface INoteRetrieverService
{
    Task<NoteDto?> GetByIdAsync(int userId, Guid noteId);
    Task<IEnumerable<NoteDto>> GetByCategoryAsync(int userId, Guid categoryGuid, int pageNumber = 0,
        int pageSize = 20);

    Task<IEnumerable<NoteDto>> GetByTagAsync(int userId, int tagId, int pageNumber = 0, int pageSize = 20);
    Task<IEnumerable<NoteDto>> GetFavoritesAsync(int userId, int pageNumber = 0, int pageSize = 20);
    Task<IEnumerable<NoteDto>> GetPinnedAsync(int userId, int pageNumber = 0, int pageSize = 20);
    Task<IEnumerable<NoteDto>> GetArchivedAsync(int userId, int pageNumber = 0, int pageSize = 20);
    Task<IEnumerable<NoteDto>> GetAllAsync(int userId, int pageNumber = 0, int pageSize = 20);
}