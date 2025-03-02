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
    ITagRepository tagRepository,
    INoteRepository noteRepository,
    ITagService tagService,
    IUserValidationService userValidationService
)
    : INoteManagerService
{
    public async Task<NoteDto> AddAsync(int userId, CreateNoteDto note)
    {
        //await userValidationService.EnsureUserValidation(userId);
        
        var context = new ValidationContext(note);
        var errors = new List<ValidationResult>();

        if (Validator.TryValidateObject(note, context, errors, true) == false)
        {
            throw new InvalidNoteException(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid email request");
        }
        
        
        var newNote = new Note()
        {
            UserId = userId,
            Title = note.Title,
            Content = note.Content,
        };
        

        Category? category = null;

        if (note.CategoryId.HasValue)
        {
            category = await categoryRepository.GetByIdAsync(userId, note.CategoryId.Value);
            
            if (category == null)
            {
                throw new CategoryNotFoundException($"Category {note.CategoryId} assigned to note was not found");            
            }
            
            newNote.CategoryId = category.CategoryId;
        }

        newNote.Title = note.Title;
        newNote.Content = note.Content;
        newNote.IsFavorite = note.IsFavorite;
        newNote.UpdatedAt = DateTime.Now;
        newNote.UserId = userId;
        newNote.CreatedAt = DateTime.UtcNow;
        newNote.UpdatedAt = DateTime.UtcNow;
        newNote.Category = null;

        var notesDto = (await noteRepository.CreateAsync(newNote)).ToDto();

        var tagsDto = await tagService.UpdateNoteTagsAsync(userId, notesDto.Id, note.Tags ?? []);

        notesDto.Tags = tagsDto;
        notesDto.Category = category?.ToDto();

        // Todo: Add resources 
        return notesDto;
    }

    public async Task<NoteDto> UpdateAsync(int userId, UpdateNoteDto note)
    {
        await userValidationService.EnsureUserValidation(userId);

        var context = new ValidationContext(note);
        var errors = new List<ValidationResult>();

        if (Validator.TryValidateObject(note, context, errors, true) == false)
        {
            throw new InvalidNoteException(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid email request");
        }
        
        if (note.NoteId == Guid.Empty)
        {
            throw new InvalidNoteIdException("NoteId cannot be empty");
        }
        
        Note? existingNote = await noteRepository.GetById(userId, note.NoteId);
        
        if (existingNote == null)
        {
            throw new NoteNotFoundException($"Note with with Id {userId} for user with Id {userId} was not found");
        }

        // note category was not changed 
        // note category chagned to null 
        // note category changed to a new category 
        Category? category = null;
        if (note.CategoryId == null)
        {
            existingNote.Category = null;
            existingNote.CategoryId = null;
        } else if(note.CategoryId != existingNote.Category?.CategoryGuid) {
            category = await categoryRepository.GetByIdAsync(userId, note.CategoryId.Value);

            if (category == null)
            {
                throw new CategoryNotFoundException($"Category {note.CategoryId} assigned to note {note.NoteId} was not found");            
            }
            
            existingNote.CategoryId = category.CategoryId;

        }

        existingNote.Title = note.Title;
        existingNote.Content = note.Content;
        existingNote.IsArchived = note.IsArchived;
        existingNote.IsFavorite = note.IsFavorite;
        existingNote.IsPinned = note.IsPinned;
        existingNote.UpdatedAt = DateTime.Now;
        existingNote.UpdatedAt = DateTime.UtcNow;
        existingNote.Category = null;
        existingNote.Tags = [];
        existingNote.Resources = [];

        var updatedNote = await noteRepository.UpdateAsync(existingNote);
        var noteDto = updatedNote.ToDto();
        
        // Add tags to note 
         var tagsDto = (await tagRepository.UpdateNoteTagsAsync(userId, note.NoteId, note.Tags == null?[] : note.Tags)).ToDtoList();
         noteDto.Tags = tagsDto;
         noteDto.Category = category?.ToDto();
         
        return noteDto;
    }

    public async Task<NoteDto?> RestoreAsync(int userId, Guid noteId)
    {
        await userValidationService.EnsureUserValidation(userId);
        
        if (noteId == Guid.Empty)
        {
            throw new InvalidNoteIdException("NoteId cannot be empty");
        }
        
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
        note.Category = null;
        note.Tags = [];
        note.Resources = [];
        
        await noteRepository.UpdateAsync(note);

        return true;
    }
}