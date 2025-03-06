using CloudPad.Core.Dtos;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services;

public class NoteRetrieverService(INoteRepository noteRepository, 
    IUserValidatorService userValidatorService): INoteRetrieverService
{
    private static (int pageNumber, int pageSize) NormalizePaginationParameters(int pageNumber, int pageSize)
    {
        int normalizedPageNumber = pageNumber <= 0 ? 1 : pageNumber;
        int normalizedPageSize = pageSize <= 0 ? 20 : pageSize;
        return (normalizedPageNumber, normalizedPageSize);
    }
    
    public async Task<NoteDto?> GetByIdAsync(int userId, Guid noteId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (noteId == Guid.NewGuid())
        {
            return null;
        }
        
        var note = (await noteRepository.GetByIdAsync(userId, noteId))?.ToDto();
        return note;
    }
    
    public async Task<IEnumerable<NoteDto>> GetAllAsync(int userId, int pageNumber = 1, int pageSize = 20)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        (pageNumber, pageSize  ) = NormalizePaginationParameters(pageNumber, pageSize);

        var notes = await noteRepository.GetAllAsync(userId, pageNumber, pageSize);
        return notes.ToDtoList();
    }
}