using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface INotePdfExportService
{ 
    byte[] GenerateAsync(List<NoteDto> notes);
}
