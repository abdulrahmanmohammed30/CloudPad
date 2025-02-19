using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class NoteValidatorService(INoteRepository noteRepository):INoteValidatorService
{
    public async Task<bool> ExistsAsync(int userId, Guid noteId)
    {
        return await noteRepository.ExistsAsync(userId, noteId);
    }
}