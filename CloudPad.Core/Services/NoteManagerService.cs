using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class NoteManagerService(
    ICategoryService categoryService,
    ICategoryRepository categoryRepository,
    ITagRepository tagRepository,
    INoteRepository noteRepository,
    ITagService tagService,
    IUserValidationService userValidationService
)
    : INoteManagerService
{
    public async Task<NoteDto> AddAsync(int userId, CreateNoteDto note)
    {
        await userValidationService.EnsureUserValidation(userId);

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
        newNote.UserId = userId;
        newNote.CreatedAt = DateTime.UtcNow;
        newNote.UpdatedAt = DateTime.UtcNow;

        NoteDto notesDto = (await noteRepository.AddAsync(userId, newNote)).ToDto();

        var tagsDto = await tagService.UpdateNoteTagsAsync(userId, notesDto.Id, note.Tags);

        notesDto.Tags = tagsDto;
        notesDto.Category = categoryDto;

        // Todo: Add resources 
        return notesDto;
    }

    public async Task<NoteDto> UpdateAsync(int userId, UpdateNoteDto note)
    {
        await userValidationService.EnsureUserValidation(userId);

        Note? existingNote = await noteRepository.GetById(userId, note.NoteId);

        if (existingNote == null)
        {
            throw new NoteNotFoundException($"Note with with Id {userId} for user with Id {userId} was not found");
        }

        // note category was not changed 
        // note category chagned to null 
        // note category changed to a new category 

        if (note.CategoryId == null)
        {
            existingNote.Category = null;
            existingNote.CategoryId = null;
        } else if(note.CategoryId != existingNote.Category?.CategoryGuid) {
            existingNote.Category = await categoryRepository.GetByIdAsync(userId, note.CategoryId.Value);

            if (existingNote.Category == null)
            {
                throw new CategoryNotFoundException($"Category {note.CategoryId} assigned to note {note.NoteId} was not found");            
            }

            existingNote.CategoryId = existingNote.Category.CategoryId;
        }

        existingNote.Title = note.Title;
        existingNote.Content = note.Content;
        existingNote.IsArchived = note.IsArchived;
        existingNote.IsFavorite = note.IsFavorite;
        existingNote.IsPinned = note.IsPinned;
        existingNote.UpdatedAt = DateTime.Now;
        existingNote.UpdatedAt = DateTime.UtcNow;

        var noteDto = (await noteRepository.UpdateAsync(existingNote)).ToDto();

        // Add tags to note 
        var tagsDto = (await tagRepository.UpdateNoteTagsAsync(userId, note.NoteId, note.Tags == null?[] : note.Tags)).ToDtoList();
        noteDto.Tags = tagsDto;

        return noteDto;
    }

    public async Task<NoteDto?> RestoreAsync(int userId, Guid noteId)
    {
        await userValidationService.EnsureUserValidation(userId);
        var note = await noteRepository.RestoreAsync(userId, noteId);
        return note?.ToDto();
    }

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

        note.IsDeleted = true;
        await noteRepository.UpdateAsync(note);

        return true;
    }
}