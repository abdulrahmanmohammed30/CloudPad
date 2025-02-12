namespace NoteTakingApp.Core.Dtos;

public class CategoryDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsFavorite { get; set; } 
}