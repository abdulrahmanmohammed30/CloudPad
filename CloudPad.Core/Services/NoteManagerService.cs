using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class NoteManagerService(
    ICategoryRepository categoryRepository,
    INoteRepository noteRepository,
    ITagManagerService tagManagerService,
    IUserValidatorService userValidatorService
)
    : INoteManagerService
{
    public async Task<NoteDto> AddAsync(int userId, CreateNoteDto createNoteDto)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        var context = new ValidationContext(createNoteDto);
        var errors = new List<ValidationResult>();

        if (Validator.TryValidateObject(createNoteDto, context, errors, true) == false)
        {
            throw new InvalidCreateNoteException(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid note data");
        }


        var newNote = new Note()
        {
            UserId = userId,
            Title = createNoteDto.Title,
            Content = createNoteDto.Content,
        };


        Category? category = null;

        if (createNoteDto.CategoryId.HasValue && createNoteDto.CategoryId != Guid.Empty)
        {
            category = await categoryRepository.GetByIdAsync(userId, createNoteDto.CategoryId.Value);

            if (category == null)
            {
                throw new CategoryNotFoundException(
                    $"Category {createNoteDto.CategoryId} assigned to note was not found");
            }

            newNote.CategoryId = category.CategoryId;
        }

        newNote.Title = createNoteDto.Title;
        newNote.Content = createNoteDto.Content;
        newNote.IsFavorite = createNoteDto.IsFavorite;
        newNote.UserId = userId;
        newNote.CreatedAt = DateTime.UtcNow;
        newNote.UpdatedAt = DateTime.UtcNow;
        newNote.Category = null;

        var noteDto = (await noteRepository.CreateAsync(newNote)).ToDto();


        var tagsDto = await tagManagerService.UpdateNoteTagsAsync(userId, noteDto.Id, createNoteDto.Tags ?? []);

        noteDto.Tags = tagsDto;
        noteDto.Category = category?.ToDto();

        // Todo: Add resources 
        return noteDto;
    }

    public async Task<NoteDto> UpdateAsync(int userId, UpdateNoteDto updateNoteDto)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        var context = new ValidationContext(updateNoteDto);
        var errors = new List<ValidationResult>();

        if (Validator.TryValidateObject(updateNoteDto, context, errors, true) == false)
        {
            throw new InvalidUpdateNoteException(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid note data");
        }

        if (updateNoteDto.NoteId == Guid.Empty)
        {
            throw new InvalidNoteIdException("Note id is invalid");
        }

        Note? existingNote = await noteRepository.GetByIdAsync(userId, updateNoteDto.NoteId);

        if (existingNote == null)
        {
            throw new NoteNotFoundException($"Note with with Id {userId} for user with Id {userId} was not found");
        }

        // note category was not changed 
        // note category changed to null 
        // note category changed to a new category 
        Category? category = null;
        if (updateNoteDto.CategoryId == null)
        {
            existingNote.Category = null;
            existingNote.CategoryId = null;
        }
        else if (updateNoteDto.CategoryId != existingNote.Category?.CategoryGuid)
        {
            category = await categoryRepository.GetByIdAsync(userId, updateNoteDto.CategoryId.Value);

            if (category == null)
            {
                throw new CategoryNotFoundException(
                    $"Category {updateNoteDto.CategoryId} assigned to note {updateNoteDto.NoteId} was not found");
            }

            existingNote.CategoryId = category.CategoryId;
        }

        existingNote.Title = updateNoteDto.Title;
        existingNote.Content = updateNoteDto.Content;
        existingNote.IsFavorite = updateNoteDto.IsFavorite;
        existingNote.IsPinned = updateNoteDto.IsPinned;
        existingNote.IsArchived = updateNoteDto.IsArchived;
        existingNote.UpdatedAt = DateTime.Now;
        existingNote.UpdatedAt = DateTime.UtcNow;
        existingNote.Category = null;
        existingNote.Tags = [];
        existingNote.Resources = [];

        var updatedNote = await noteRepository.UpdateAsync(existingNote);
        var noteDto = updatedNote.ToDto();

        // Add tags to note 
        var tagsDto = await tagManagerService.UpdateNoteTagsAsync(userId, noteDto.Id, updateNoteDto.Tags ?? []);

        // var tagsDto = (await tagService.UpdateNoteTagsAsync(userId, updateNoteDto.NoteId,
        //     updateNoteDto.Tags ?? [])).ToDtoList();
        noteDto.Tags = tagsDto;
        noteDto.Category = category?.ToDto();

        return noteDto;
    }

    public async Task<NoteDto?> RestoreAsync(int userId, Guid noteId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (noteId == Guid.Empty)
        {
            throw new InvalidNoteIdException("Note id is invalid");
        }

        var note = await noteRepository.RestoreAsync(userId, noteId);
        return note?.ToDto();
    }

    public async Task DeleteAsync(int userId, Guid noteId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (noteId == Guid.Empty)
        {
            throw new InvalidNoteIdException("Note id is invalid");
        }

        var note = await noteRepository.GetByIdAsync(userId, noteId);

        if (note == null)
        {
            throw new NoteNotFoundException($"Note with with Id {userId} for user with Id {userId} was not found");
        }

        note.IsDeleted = true;
        note.Category = null;
        note.Tags = [];
        note.Resources = [];

        await noteRepository.UpdateAsync(note);
    }
}