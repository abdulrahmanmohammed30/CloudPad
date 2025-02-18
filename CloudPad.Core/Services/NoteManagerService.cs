using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class NoteManagerService(
    ICategoryService categoryService,
    INoteRepository noteRepository,
    ITagService tagService,
    IUserValidationService userValidationService
)
    : INoteManagerService
{
    public async Task<NoteDto> AddAsync(int userId, CreateNoteDto note)
    {
        var newNote = new Note()
        {
            UserId = userId,
            Title = note.Title,
            Content = note.Content,
        };

        CategoryDto? categoryDto = null;
        int? categoryId = null;
        if (note.CategoryId.HasValue)
        {
            var categoryNoteTask = categoryService.FindCategoryIdByGuidAsync(userId, note.CategoryId.Value);
            var categoryDtoTask = categoryService.GetByIdAsync(userId, note.CategoryId.Value);
            await Task.WhenAll(categoryNoteTask, categoryDtoTask);
            categoryId = await categoryNoteTask;
            categoryDto = await categoryDtoTask;
        }

        newNote.Title = note.Title;
        newNote.Content = note.Content;
        newNote.IsFavorite = note.IsFavorite;
        newNote.UpdatedAt = DateTime.Now;
        newNote.CategoryId = categoryId;

        var notesDto = (await noteRepository.AddAsync(userId, newNote)).ToDto();

        var tagsDto = await tagService.UpdateNoteTagsAsync(userId, notesDto.Id, note.Tags);

        notesDto.Tags = tagsDto;
        notesDto.Category = categoryDto;

        // Todo: Add resources 
        return notesDto;
    }

    public async Task<NoteDto> UpdateAsync(int userId, UpdateNoteDto note)
    {
        Note? existingNote = null;
        var existingNoteTask = noteRepository.GetById(userId, note.NoteId);
        CategoryDto? categoryDto = null;
        int? categoryId = null;
        if (note.CategoryId.HasValue)
        {
            var categoryNoteTask = categoryService.FindCategoryIdByGuidAsync(userId, note.CategoryId.Value);
            var categoryDtoTask = categoryService.GetByIdAsync(userId, note.CategoryId.Value);
            await Task.WhenAll(existingNoteTask, categoryNoteTask, categoryDtoTask);
            existingNote = await existingNoteTask;
            categoryId = await categoryNoteTask;
            categoryDto = await categoryDtoTask;
        }
        else
        {
            existingNote = await existingNoteTask;
        }

        if (existingNote == null)
        {
            throw new NoteNotFoundException($"Note with with Id {userId} for user with Id {userId} was not found");
        }

        // Add tags to note 
        var tagsDto = await tagService.UpdateNoteTagsAsync(userId, note.NoteId, note.Tags);

        existingNote.Title = note.Title;
        existingNote.Content = note.Content;
        existingNote.IsArchived = note.IsArchived;
        existingNote.IsFavorite = note.IsFavorite;
        existingNote.IsPinned = note.IsPinned;
        existingNote.UpdatedAt = DateTime.Now;
        existingNote.CategoryId = categoryId;

        var notesDto = (await noteRepository.UpdateAsync(existingNote)).ToDto();
        notesDto.Tags = tagsDto;
        notesDto.Category = categoryDto;

        // Todo: Add resources 
        return notesDto;
    }

    public async Task<NoteDto?> RestoreAsync(int userId, Guid noteId)
    {
        await userValidationService.EnsureUserValidation(userId);
        var note = await noteRepository.RestoreAsync(userId, noteId);
        return note?.ToDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="noteId"></param>
    /// <returns></returns>
    /// <exception cref="NoteNotFoundException"></exception>
    public async Task<bool> DeleteAsync(int userId, Guid noteId)
    {
        await userValidationService.EnsureUserValidation(userId);

        if (noteId == Guid.Empty)
        {
            throw new InvalidNoteIdException("NoteId cannot be empty");
        }

        var note = await noteRepository.GetById(userId, noteId);

        if (note == null)
        {
            throw new NoteNotFoundException($"Note with with Id {userId} for user with Id {userId} was not found");
        }

        return await noteRepository.DeleteAsync(userId, noteId);
    }
}