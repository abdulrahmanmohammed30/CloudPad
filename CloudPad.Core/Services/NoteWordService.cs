// using CloudPad.Core.Dtos;
// using CloudPad.Core.ServiceContracts;
// using DocumentFormat.OpenXml;
// using DocumentFormat.OpenXml.Drawing;
// using DocumentFormat.OpenXml.Drawing.Wordprocessing;
// using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Wordprocessing;
// using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
// using NonVisualGraphicFrameDrawingProperties = DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties;
// using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
// using Picture = DocumentFormat.OpenXml.Wordprocessing.Picture;
// using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
// using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
// using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
//
// namespace NoteTakingApp.Core.Services;
//
// public class NoteWordExportService : INoteWordExportService
// {
//     public byte[] GenerateAsync(IEnumerable<NoteDto> notes)
//     {
//         using (MemoryStream memoryStream = new MemoryStream())
//         {
//             using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true))
//             {
//                 MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
//                 mainPart.Document = new Document();
//                 Body body = new Body();
//                 
//                 foreach (var note in notes)
//                 {
//                     // Title
//                     body.Append(CreateParagraph(note.Title, 24, true));
//                     
//                     // Content
//                     body.Append(CreateParagraph(note.Content ?? "No content", 12, false));
//                     
//                     // Tags
//                     string tagString = string.Join(", ", note.Tags.Select(t => t.Name));
//                     body.Append(CreateParagraph("Tags: " + tagString, 12, false));
//                     
//                     
//                     // Images
//                     if (note.Resources.Any())
//                     {
//                         body.Append(CreateParagraph("Resources:", 14, true));
//                         foreach (var resource in note.Resources)
//                         {
//                             body.Append(CreateParagraph($"{resource.DisplayName}: {resource.Description}", 12, false));
//                             AddImageToDocument(mainPart, body, resource.FilePath);
//                         }
//                     }
//                     
//                     // Page Break
//                     body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
//                 }
//                 
//                 mainPart.Document.Append(body);
//                 mainPart.Document.Save();
//             }
//             
//             return memoryStream.ToArray();
//         }
//     }
//
//     private Paragraph CreateParagraph(string text, int fontSize, bool isBold)
//     {
//         return new Paragraph(
//             new Run(
//                 new RunProperties(
//                     new FontSize() { Val = (fontSize * 2).ToString() },
//                     isBold ? new Bold() : null
//                 ),
//                 new Text(text)
//             )
//         );
//     }
//     
//     private void AddImageToDocument(MainDocumentPart mainPart, Body body, string imagePath)
//     {
//         ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
//         using (FileStream stream = new FileStream(imagePath, FileMode.Open))
//         {
//             imagePart.FeedData(stream);
//         }
//
//         var imageId = mainPart.GetIdOfPart(imagePart);
//
//         var element = new Drawing(
//             new Inline(
//                 new Extent() { Cx = 990000L, Cy = 792000L },
//                 new EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
//                 new DocProperties() { Id = (UInt32Value)1U, Name = "Picture" },
//                 new NonVisualGraphicFrameDrawingProperties(new GraphicFrameLocks() { NoChangeAspect = true }),
//                 new Graphic(
//                     new GraphicData(
//                         new Picture(
//                             new NonVisualPictureProperties(
//                                 new NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "New Bitmap Image.jpg" },
//                                 new NonVisualPictureDrawingProperties()
//                             ),
//                             new BlipFill(
//                                 new Blip() { Embed = imageId },
//                                 new Stretch(new FillRectangle())
//                             ),
//                             new ShapeProperties(
//                                 new Transform2D(
//                                     new Offset() { X = 0L, Y = 0L },
//                                     new Extents() { Cx = 990000L, Cy = 792000L }
//                                 ),
//                                 new PresetGeometry(new AdjustValueList()) { Preset = ShapeTypeValues.Rectangle }
//                             )
//                         )
//                     ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
//                 )
//             )
//         );
//
//         body.AppendChild(new Paragraph(new Run(element)));
//     }
// }


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using CloudPad.Core.Dtos;
using CloudPad.Core.ServiceContracts;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

public class NoteWordExportService : INoteWordExportService
{
    private readonly string TitleColor = "2B579A";  // Blue color for titles
    private readonly int TitleSize = 16;            // Title font size
    private readonly int NormalSize = 11;           // Normal text size
    
    public byte[] GenerateAsync(IEnumerable<NoteDto> notes)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                foreach (var note in notes)
                {
                    // Add note title
                    AddNoteTitle(body, note.Title);

                    // Add content if exists
                    if (!string.IsNullOrEmpty(note.Content))
                    {
                        AddPropertyWithLabel(body, "Content", note.Content);
                    }

                    // Add category if exists
                    if (note.Category != null)
                    {
                        AddPropertyWithLabel(body, "Category", note.Category.Name);
                    }

                    // Add tags if any
                    if (note.Tags.Any())
                    {
                        string tags = string.Join(", ", note.Tags.Select(t => t.Name));
                        AddPropertyWithLabel(body, "Tags", tags);
                    }

                    // Add status flags if true
                    if (note.IsFavorite) AddPropertyWithLabel(body, "Status", "Favorite");
                    if (note.IsPinned) AddPropertyWithLabel(body, "Status", "Pinned");
                    if (note.IsArchived) AddPropertyWithLabel(body, "Status", "Archived");

                    // Add resources/images if any
                    if (note.Resources.Any())
                    {
                        foreach (var resource in note.Resources)
                        {
                            if (File.Exists(resource.FilePath))
                            {
                                AddImage(body, mainPart, resource.FilePath, resource.DisplayName ?? "Image");
                            }
                        }
                    }

                    // Add spacing between notes
                    AddSpacing(body);
                }
            }

            return ms.ToArray();
        }
    }

    private void AddNoteTitle(Body body, string title)
    {
        Paragraph para = body.AppendChild(new Paragraph());
        Run run = para.AppendChild(new Run());
        RunProperties runProps = run.AppendChild(new RunProperties());
        
        runProps.AppendChild(new Color { Val = TitleColor });
        runProps.AppendChild(new FontSize { Val = (TitleSize * 2).ToString() });
        runProps.AppendChild(new Bold());
        
        run.AppendChild(new Text(title));
    }

    private void AddPropertyWithLabel(Body body, string label, string value)
    {
        Paragraph para = body.AppendChild(new Paragraph());
        
        // Add label in bold
        Run labelRun = para.AppendChild(new Run());
        RunProperties labelProps = labelRun.AppendChild(new RunProperties());
        labelProps.AppendChild(new Bold());
        labelProps.AppendChild(new FontSize { Val = (NormalSize * 2).ToString() });
        labelRun.AppendChild(new Text($"{label}: "));

        // Add value in normal text
        Run valueRun = para.AppendChild(new Run());
        RunProperties valueProps = valueRun.AppendChild(new RunProperties());
        valueProps.AppendChild(new FontSize { Val =(NormalSize * 2).ToString() });
        valueRun.AppendChild(new Text(value));
    }

    private void AddImage(Body body, MainDocumentPart mainPart, string imagePath, string description)
    {
        ImagePart imagePart = mainPart.AddImagePart(GetImagePartType(imagePath));
        
        using (FileStream stream = new FileStream(imagePath, FileMode.Open))
        {
            imagePart.FeedData(stream);
        }

        AddImageToBody(body, mainPart.GetIdOfPart(imagePart), description);
    }

    private PartTypeInfo GetImagePartType(string imagePath)
    {
        string extension = Path.GetExtension(imagePath).ToLower();
        return extension switch
        {
            ".png" => ImagePartType.Png,
            ".jpg" or ".jpeg" => ImagePartType.Jpeg,
            _ => throw new NotSupportedException($"Image type {extension} not supported")
        };
    }

    private void AddImageToBody(Body body, string relationshipId, string description)
    {
        long imageWidth = 5000000L;  // 5 inches
        long imageHeight = 3000000L; // 3 inches

        var element =
            new Drawing(
                new DW.Inline(
                    new DW.Extent() { Cx = imageWidth, Cy = imageHeight },
                    new DW.EffectExtent()
                    {
                        LeftEdge = 0L,
                        TopEdge = 0L,
                        RightEdge = 0L,
                        BottomEdge = 0L
                    },
                    new DW.DocProperties()
                    {
                        Id = 1U,
                        Name = description
                    },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                        new A.GraphicFrameLocks() { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties()
                                    {
                                        Id = 0U,
                                        Name = description
                                    },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip(
                                        new A.BlipExtensionList(
                                            new A.BlipExtension()
                                            {
                                                Uri = "{28A0092B-C50C-407E-A947-70E740481C3C}"
                                            }))
                                    {
                                        Embed = relationshipId,
                                        CompressionState = A.BlipCompressionValues.Print
                                    },
                                    new A.Stretch(
                                        new A.FillRectangle())),
                                new PIC.ShapeProperties(
                                    new A.Transform2D(
                                        new A.Offset() { X = 0L, Y = 0L },
                                        new A.Extents() { Cx = imageWidth, Cy = imageHeight }),
                                    new A.PresetGeometry(
                                        new A.AdjustValueList()
                                    )
                                    { Preset = A.ShapeTypeValues.Rectangle })))
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
                {
                    DistanceFromTop = 0U,
                    DistanceFromBottom = 0U,
                    DistanceFromLeft = 0U,
                    DistanceFromRight = 0U
                });

        body.AppendChild(new Paragraph(new Run(element)));
    }

    private void AddSpacing(Body body)
    {
        Paragraph spacing = body.AppendChild(new Paragraph());
        spacing.AppendChild(new Run(new Break()));
        spacing.AppendChild(new Run(new Break()));
    }
}
