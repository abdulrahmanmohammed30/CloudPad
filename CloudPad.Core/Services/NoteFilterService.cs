using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;
using CloudPad.Core.Enums;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;
using NoteTakingApp.Core.Enums;

namespace CloudPad.Core.Services;

public class NoteFilterService(
    INoteRepository noteRepository,
    IUserValidatorService userValidatorService)
    : INoteFilterService
{
    private const int PageNumber = 1;
    private const int PageSize = 15;

    private static (int pageNumber, int pageSize) NormalizePaginationParameters(int pageNumber, int pageSize)
    {
        int normalizedPageNumber = pageNumber <= 0 ? PageNumber : pageNumber;
        int normalizedPageSize = pageSize <= 0 ? PageSize : pageSize;
        return (normalizedPageNumber, normalizedPageSize);
    }

    public async Task<IEnumerable<NoteDto>> SearchAsync(int userId,  
        string searchTerm,
        SearchFields searchFields,
        int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);
        (pageNumber, pageSize) = NormalizePaginationParameters(pageNumber, pageSize);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize)).Select(n => n.ToDto());
        }

        return (await noteRepository.SearchAsync(userId, searchTerm, pageNumber, pageSize)).ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> SearchByTitleAsync(int userId, string searchTerm,
        int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        return await SearchAsync(userId, searchTerm, SearchFields.Title, pageNumber, pageSize);
    }

    public async Task<IEnumerable<NoteDto>> SearchByContentAsync(int userId, string searchTerm,
        int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        return await SearchAsync(userId, searchTerm, SearchFields.Content, pageNumber, pageSize);
    }

    private bool CanConvert(Type type, string value)
    {
        try
        {
            _ = Convert.ChangeType(value, type);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<NoteDto>> FilterByAsync(int userId, string column, string value,
        int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);
        
        (pageNumber, pageSize) = NormalizePaginationParameters(pageNumber, pageSize);

        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentNullException(nameof(column), "Search column cannot be null or empty");
        }

        var isSearchableColumn = Enum.TryParse(typeof(NoteSearchableColumn), column, out var searchableColumn);
        if (!isSearchableColumn)
        {
            throw new InvalidSearchColumnException("column is not searchable");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize)).Select(n => n.ToDto());
        }

        var type = typeof(Note).GetProperty(searchableColumn.ToString()).PropertyType;
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (CanConvert(type, value) == false)
        {
            throw new InvalidSearchValueException($"value {value} is not assignable to column {column}");
        }


        return (await noteRepository.FilterAsync(userId, (NoteSearchableColumn)searchableColumn, value, pageNumber,
            pageSize)).ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> FilterAsync(int userId, 
        string title = "", string content = "",
        string tag = "", string category = "",
        bool isFavorite = false, bool isPinned = false,
        bool isArchived = false,
        int pageNumber = PageNumber, int pageSize = PageSize)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        (pageNumber, pageSize) = NormalizePaginationParameters(pageNumber, pageSize);


        return (await noteRepository.FilterAsync(userId, title, content, tag, category, isFavorite, isPinned,
            isArchived, pageNumber, pageSize)).ToDtoList();
    }
}