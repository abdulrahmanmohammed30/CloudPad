using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Enums;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class NoteFilterService(IUserService userService, INoteRepository noteRepository, IUserValidationService userValidationServiceS)
    :INoteFilterService
{
    public async Task<IEnumerable<NoteDto>> SearchAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20)
    {
        await userValidationServiceS.EnsureUserValidation(userId);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize))
                .Select(n => n.ToDto());
        }

        return (await noteRepository.SearchAsync(userId, searchTerm, pageNumber, pageSize))
            .ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20)
    {
        await userValidationServiceS.EnsureUserValidation(userId);


        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize))
                .Select(n => n.ToDto());
        }

        return (await noteRepository.SearchByTitleAsync(userId, searchTerm, pageNumber, pageSize))
            .ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20)
    {
        await userValidationServiceS.EnsureUserValidation(userId);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize))
                .Select(n => n.ToDto());
        }

        return (await noteRepository.SearchByContentAsync(userId, searchTerm, pageNumber, pageSize))
            .ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> FilterAsync(int userId, string column, string value, int pageNumber = 0,
        int pageSize = 20)
    {
        await userValidationServiceS.EnsureUserValidation(userId);

        var isSearchableColumn = (Enum.GetNames(typeof(NoteSearchableColumn))).Any(n => n.Equals(column));
        if (!isSearchableColumn)
        {
            throw new NoteColumnNotSearchable("Not searchable column");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize)).Select(n => n.ToDto());
        }

        var searchableColumn = (NoteSearchableColumn)Enum.Parse(typeof(NoteSearchableColumn), column, true);

        //Expression<Func<NoteDto, bool>> filter 
        return (await noteRepository.FilterAsync(userId, searchableColumn, value, pageNumber, pageSize))
            .ToDtoList();
    }
}