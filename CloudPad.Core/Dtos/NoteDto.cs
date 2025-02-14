namespace NoteTakingApp.Core.Dtos;

public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public List<TagDto> Tags { get; set; } = new List<TagDto>();
    public CategoryDto? Category { get; set; }

    public bool IsFavorite { get; set; } = false;
    public bool IsPinned { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    
    public List<ResourceDto> Resources { get; set; } = new List<ResourceDto>();
}