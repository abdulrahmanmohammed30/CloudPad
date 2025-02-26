using PdfSharp.Drawing;
using PdfSharp.Pdf;
using CloudPad.Core.Entities;
using System.IO;
using CloudPad.Core.Dtos;
using CloudPad.Core.ServiceContracts;


public class NotePdfExportService:INotePdfExportService
{
    public byte[] GenerateAsync(List<NoteDto> notes)
    {
        using (var stream = new MemoryStream())
        {
            PdfDocument document = new PdfDocument();
            int pageNumber = 1;

            foreach (var note in notes)
            {   
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
                XFont contentFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                XFont smallFont = new XFont("Arial", 10, XFontStyleEx.Italic);
                XFont tagFont = new XFont("Arial", 10, XFontStyleEx.Bold);

                double yPosition = 40;
                double marginLeft = 40;
                double lineHeight = 20;

                // Add Note Title
                gfx.DrawString($"Title: {note.Title}", titleFont, XBrushes.Black, new XRect(marginLeft, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight * 2;

                // Add Note Content
                gfx.DrawString(note.Content ?? "No content available.", contentFont, XBrushes.Black, new XRect(marginLeft, yPosition, page.Width - 80, page.Height - 80), XStringFormats.TopLeft);
                yPosition += lineHeight * 3;

                // Add Tags
                if (note.Tags.Any())
                {
                    gfx.DrawString("Tags: " + string.Join(", ", note.Tags.Select(t => t.Name)), tagFont, XBrushes.Gray, new XRect(marginLeft, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += lineHeight;
                }

                // Add Category
                if (note.Category != null)
                {
                    gfx.DrawString($"Category: {note.Category.Name}", smallFont, XBrushes.Gray, new XRect(marginLeft, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += lineHeight;
                }


                // Add Images (Resources)
                foreach (var resource in note.Resources)
                {
                    if (File.Exists(resource.FilePath))
                    {
                        try
                        {
                            XImage image = XImage.FromFile(resource.FilePath);
                            double maxWidth = page.Width - 80;
                            double maxHeight = 200;
                            double width = image.PixelWidth;
                            double height = image.PixelHeight;
                            double scale = Math.Min(maxWidth / width, maxHeight / height);

                            width *= scale;
                            height *= scale;

                            gfx.DrawString($"Image: {resource.DisplayName ?? "Unnamed"}", smallFont, XBrushes.Black, new XRect(marginLeft, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                            yPosition += lineHeight;
                            gfx.DrawImage(image, marginLeft, yPosition, width, height);
                            yPosition += height + 10;

                            if (!string.IsNullOrWhiteSpace(resource.Description))
                            {
                                gfx.DrawString($"Description: {resource.Description}", smallFont, XBrushes.Gray, new XRect(marginLeft, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                                yPosition += lineHeight * 2;
                            }
                        }
                        catch (Exception ex)
                        {
                            gfx.DrawString($"Error loading image: {resource.FilePath}", smallFont, XBrushes.Red, new XRect(marginLeft, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                            yPosition += lineHeight * 2;
                        }
                    }
                }

                // Add Page Number
                gfx.DrawString($"Page {pageNumber}", smallFont, XBrushes.Black, new XRect(0, page.Height - 40, page.Width, page.Height), XStringFormats.BottomCenter);
                pageNumber++;
            }

            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}
