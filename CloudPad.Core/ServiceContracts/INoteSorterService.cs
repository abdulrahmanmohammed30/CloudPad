using NoteTakingApp.Core.Dtos;

namespace NoteTakingApp.Core.ServiceContracts;

public interface INoteSorterService
{
    Task<IEnumerable<NoteDto>> SortAsync(int userId, string column, bool sortDescending = true,int pageNumber = 0,
        int pageSize = 20);
}