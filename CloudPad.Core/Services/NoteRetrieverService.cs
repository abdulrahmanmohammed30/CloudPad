using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class NoteRetrieverService(INoteRepository noteRepository, ICategoryService categoryService, ITagService tagService,
    IUserValidationService userValidationService): INoteRetrieverService
{
    private async Task ValidateCategoryAsync(int userId, Guid categoryId)
    {   
        await userValidationService.EnsureUserValidation(userId);

        if (categoryId == Guid.Empty)
        {
            throw new InvalidCategoryException("Category id cannot be empty");
        }

        if (!await categoryService.ExistsAsync(userId, categoryId))
        {
            throw new CategoryNotFoundException($"Category with id {categoryId} doesn't exist");
        }
    }
    
    private async Task ValidateTagAsync(int userId, int tagId)
    {
        await userValidationService.EnsureUserValidation(userId);

        if (tagId <=0)
            throw new InvalidTagException("Tag id cannot be less than or equal 0");

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

    public async Task<IEnumerable<NoteDto>> GetByCategoryAsync(int userId, Guid categoryId, int pageNumber = 1,
        int pageSize = 20)
    {
        Task.WaitAll(userValidationService.EnsureUserValidation(userId), ValidateCategoryAsync(userId, categoryId));

        var notes = await noteRepository.GetByCategoryAsync(userId, categoryId, pageNumber, pageSize);

        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetByTagAsync(int userId, int tagId, int pageNumber = 1, int pageSize = 20)
    {
        Task.WaitAll(userValidationService.EnsureUserValidation(userId), 
            ValidateTagAsync(userId, tagId));

        var notes = await noteRepository.GetByTagAsync(userId, tagId, pageNumber, pageSize);
        ;
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetFavoritesAsync(int userId, int pageNumber = 1, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetFavoritesAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetPinnedAsync(int userId, int pageNumber = 1, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetPinnedAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetArchivedAsync(int userId, int pageNumber = 1, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetArchivedAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync(int userId, int pageNumber = 1, int pageSize = 20)
    {
        await userValidationService.EnsureUserValidation(userId);

        var notes = await noteRepository.GetAllAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }
}