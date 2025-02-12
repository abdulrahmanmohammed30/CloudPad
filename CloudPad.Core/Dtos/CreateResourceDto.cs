using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class CreateResourceDto
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? DisplayName { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }

    public long? Size { get; set; }
    
    public int NoteId { get; set; }
}
