using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class NoteRetrieverService(INoteRepository noteRepository, ICategoryService categoryService, ITagService tagService,
    IUserValidationService userValidationService): INoteRetrieverService
{
    private async Task ValidateCategoryAsync(int userId, Guid categoryId)
    {
        if (!await categoryService.ExistsAsync(userId, categoryId))
        {
            throw new CategoryNotFoundException($"Category with id {categoryId} doesn't exist");
        }
    }
    
    private async Task ValidateTagAsync(int userId, int tagId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tagId);

        if (!await tagService.ExistsAsync(userId, tagId))
        {
            throw new TagNotFoundException($"Tag with id {tagId} doesn't exist");
        }
    }
    

    public async Task<NoteDto?> GetByIdAsync(int userId, Guid noteId)
    {
        await userValidationService.EnsureUserValidation(userId);
        var note = (await noteRepository.GetById(userId, noteId))?.ToDto();
        return note;
    }

    public async Task<IEnumerable<NoteDto>> GetByCategoryAsync(int userId, Guid categoryId, int pageNumber = 0,
        int pageSize = 20)
    {
        Task.WaitAll(userValidationService.EnsureUserValidation(userId), ValidateCategoryAsync(userId, categoryId));

        var notes = await noteRepository.GetByCategoryAsync(userId, categoryId, pageNumber, pageSize);

        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetByTagAsync(int userId, int tagId, int pageNumber = 0, int pageSize = 20)
    {
        Task.WaitAll(userValidationService.EnsureUserValidation(userId), 
            ValidateTagAsync(userId, tagId));

        var notes = await noteRepository.GetByTagAsync(userId, tagId, pageNumber, pageSize);
        ;
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetFavoritesAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetFavoritesAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetPinnedAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetPinnedAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetArchivedAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetArchivedAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetAllAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }
}