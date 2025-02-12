using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class UpdateNoteDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(150)]
    public Guid NoteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public Guid? CategoryId { get; set; }
    public List<int> Tags { get; set; } = new List<int>();

    public bool IsPinned { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    public bool IsFavorite { get; set; } = false;
    
}

