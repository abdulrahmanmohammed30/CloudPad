using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface INoteSorterService
{
    Task<IEnumerable<NoteDto>> SortAsync(int userId, string column, bool sortDescending = true,int pageNumber = 0,
        int pageSize = 20);
}