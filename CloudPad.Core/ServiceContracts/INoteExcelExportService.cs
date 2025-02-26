using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;


public interface INoteExcelExportService
{
    MemoryStream GenerateAsync(List<NoteDto> notes);
}   
