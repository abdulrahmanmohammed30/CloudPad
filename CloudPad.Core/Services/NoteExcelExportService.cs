using CloudPad.Core.Dtos;
using CloudPad.Core.ServiceContracts;
using OfficeOpenXml;

namespace NoteTakingApp.Core.Services;

public class NoteExcelExportService : INoteExcelExportService
{
    public MemoryStream GenerateAsync(List<NoteDto> notes)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var stream = new MemoryStream();
        using (var package = new ExcelPackage(stream))
        {
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("Notes");

            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Title";
            worksheet.Cells[1, 3].Value = "Content";
            worksheet.Cells[1, 4].Value = "Tags";
            worksheet.Cells[1, 5].Value = "Category";
            worksheet.Cells[1, 6].Value = "IsFavorite";
            worksheet.Cells[1, 7].Value = "IsPinned";
            worksheet.Cells[1, 8].Value = "IsArchived";

            using (var headerRange = worksheet.Cells[1, 1, 1, 8])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            for (int i = 0; i < notes.Count(); i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = notes[i].Title;
                worksheet.Cells[i + 2, 3].Value = notes[i].Content;
                worksheet.Cells[i + 2, 4].Value = string.Join(", ", notes[i].Tags.Select(t => t.Name));
                worksheet.Cells[i + 2, 5].Value = notes[i].Category?.Name;
                worksheet.Cells[i + 2, 6].Value = notes[i].IsFavorite;
                worksheet.Cells[i + 2, 7].Value = notes[i].IsPinned;
                worksheet.Cells[i + 2, 8].Value = notes[i].IsArchived;
            }

            package.Save();
        }

        // worksheet.Cells.AutoFitColumns();
        // return await package.GetAsByteArrayAsync();
        stream.Position = 0;
        return stream;
    }
}