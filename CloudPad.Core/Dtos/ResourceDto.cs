namespace NoteTakingApp.Core.Dtos;

public class ResourceDto
{
    public Guid ResourceId { get; set; } 
    
    public string FilePath { get; set; } = string.Empty;
    
    public string? DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public long? Size { get; set; }
}

