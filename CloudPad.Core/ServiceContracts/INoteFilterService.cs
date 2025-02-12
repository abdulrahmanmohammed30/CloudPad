using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.ServiceContracts;

public interface INoteFilterService
{
    Task<IEnumerable<NoteDto>> SearchAsync(int userId, string searchTerm, int pageNumber = 0, int pageSize = 20);

    Task<IEnumerable<NoteDto>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20);

    Task<IEnumerable<NoteDto>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20);

    Task<IEnumerable<NoteDto>> FilterAsync(int userId, string column, string value, int pageNumber = 0,
        int pageSize = 20);
}