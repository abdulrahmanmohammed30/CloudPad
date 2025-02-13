using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class UpdateNoteDto
{
    public Guid NoteId { get; set; }
        
    [Required(AllowEmptyStrings = false)]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public Guid? CategoryId { get; set; }
    public List<int>? Tags { get; set; }

    public bool IsPinned { get; set; }
    public bool IsArchived { get; set; }
    public bool IsFavorite { get; set; }
    
}

