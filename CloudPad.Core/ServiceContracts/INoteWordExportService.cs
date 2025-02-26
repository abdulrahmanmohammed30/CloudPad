using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface INoteWordExportService
{
    byte[] GenerateAsync(IEnumerable<NoteDto> notes);
}
