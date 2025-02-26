using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Enums;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Newtonsoft.Json.Linq;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class NoteFilterService(INoteRepository noteRepository,
    IUserValidationService userValidationService)
    :INoteFilterService
{
    private const int PageNumber = 0;
    private const int PageSize = 15;
    public async Task<IEnumerable<NoteDto>> SearchAsync(int userId, string searchTerm, int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        await userValidationService.EnsureUserValidation(userId);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize))
                .Select(n => n.ToDto());
        }

        if (pageNumber < 0) pageNumber = 0;
        if (pageSize < 0) pageSize = 15;
        
        return (await noteRepository.SearchAsync(userId, searchTerm, pageNumber, pageSize))
            .ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        await userValidationService.EnsureUserValidation(userId);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize))
                .Select(n => n.ToDto());
        }

        return (await noteRepository.SearchByTitleAsync(userId, searchTerm, pageNumber, pageSize))
            .ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        await userValidationService.EnsureUserValidation(userId);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return (await noteRepository.GetAllAsync(userId, pageNumber, pageSize))
                .Select(n => n.ToDto());
        }

        return (await noteRepository.SearchByContentAsync(userId, searchTerm, pageNumber, pageSize))
            .ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> FilterAsync(int userId, string column, string value, int pageNumber = PageNumber,
        int pageSize = PageSize)
    {
        await userValidationService.EnsureUserValidation(userId);

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

    public async Task<IEnumerable<NoteDto>> FilterAsync(int userId, string title="", string content="", string tag = "", string category = "",
        bool IsFavorite = false, bool IsPinned = false, bool IsArchived = false, int pageNumber = PageNumber, int pageSize = PageSize)
    {
        await userValidationService.EnsureUserValidation(userId);


        return (await noteRepository.FilterAsync(userId, title, content, tag, category, IsFavorite,
            IsPinned, IsArchived, pageNumber, pageSize))
    .ToDtoList();

    }
}

class SearchRequest
{
    [Required]
    [Range(1, int.MaxValue)] 
    public int UserId { get; set; }

    public string SearchTerm { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 0;
    
    [Range(1, int.MaxValue)] 
    public int PageSize { get; set; } = 20;

    public SearchFields SearchFields { get; set; } 
}

enum SearchFields
{
    Title = 1,
    Content = 2,
    All = Title | Content
}