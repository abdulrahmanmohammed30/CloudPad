using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;

namespace CloudPad.Core.Services;

public class NoteValidatorService(INoteRepository noteRepository):INoteValidatorService
{
    public async Task<bool> ExistsAsync(int userId, Guid noteId)
    {
        if (userId <= 0 || noteId == Guid.Empty)
        {
            return false;
        }
        
        return await noteRepository.ExistsAsync(userId, noteId);
    }
}