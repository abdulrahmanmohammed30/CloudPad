using System.ComponentModel.DataAnnotations;

namespace CloudPad.Core.Dtos;

public class CreateNoteDto
{   
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    
    public string? Content { get; set; }

    public List<int>? Tags { get; set; }
    public Guid? CategoryId { get; set; }

    public bool IsFavorite { get; set; } = false;
}

