using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Enums;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class NoteSorterService(INoteRepository noteRepository, IUserValidationService userValidationService)
:INoteSorterService
{
    public async Task<IEnumerable<NoteDto>> SortAsync(int userId, string column, bool sortDescending = true,
        int pageNumber = 0,
        int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var isSortableColumn = (Enum.GetNames(typeof(NoteSortableColumn))).Any(n => n.Equals(column));
        if (!isSortableColumn)
        {
            throw new NoteColumnNotSortable("Not sortable column");
        }

        var sortableColumn = (NoteSortableColumn)Enum.Parse(typeof(NoteSortableColumn), column, true);
        return (await noteRepository.SortAsync(userId, sortableColumn, sortDescending, pageNumber, pageSize))
            .ToDtoList();
    }
}