using System.ComponentModel.DataAnnotations;
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
   private static (int pageNumber, int pageSize) NormalizePaginationParameters(int pageNumber, int pageSize)
    {
        int normalizedPageNumber = pageNumber <= 0 ? 1 : pageNumber;
        int normalizedPageSize = pageSize <= 0 ? 20 : pageSize;
        return (normalizedPageNumber, normalizedPageSize);
    }

    public async Task<IEnumerable<NoteDto>> SortAsync(int userId, string column, bool sortDescending = true,
        int pageNumber = 1,
        int pageSize = 20)
    
    {
        await userValidationService.EnsureUserValidation(userId);
        (pageNumber, pageSize  ) = NormalizePaginationParameters(pageNumber, pageSize);
        
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentNullException(nameof(column), "searchable column cannot be empty");
        }
        
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