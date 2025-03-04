using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface INoteManagerService
{
    Task<NoteDto> AddAsync(int userId, CreateNoteDto note);
    Task<NoteDto> UpdateAsync(int userId, UpdateNoteDto note);
    Task<NoteDto?> RestoreAsync(int userId, Guid noteId);
    Task DeleteAsync(int userId, Guid noteId);
}