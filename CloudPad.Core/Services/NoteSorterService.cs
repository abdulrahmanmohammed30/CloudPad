using CloudPad.Core.Dtos;
using CloudPad.Core.Enums;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

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