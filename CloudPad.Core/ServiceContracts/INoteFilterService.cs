using CloudPad.Core.Dtos;
using CloudPad.Core.Services;
using NoteTakingApp.Core.Enums;

namespace CloudPad.Core.ServiceContracts;

public interface INoteFilterService
{
    Task<IEnumerable<NoteDto>> SearchAsync(int userId, string searchTerm,
        SearchFields searchFields, int pageNumber = 0, int pageSize = 20);

    Task<IEnumerable<NoteDto>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20);

    Task<IEnumerable<NoteDto>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20);

    Task<IEnumerable<NoteDto>> FilterByAsync(int userId, string column, string value, int pageNumber = 0,
        int pageSize = 20);

    Task<IEnumerable<NoteDto>> FilterAsync(int userId, string title, string content, string tag, string category, bool IsFavorite,
        bool IsPinned, bool IsArchived, int pageNumber = 0, int pageSize = 20);
}