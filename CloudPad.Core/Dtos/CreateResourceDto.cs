using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class CreateResourceDto
{
    [MaxLength(255)]
    public string? DisplayName { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public Guid NoteId { get; set; }

    public IFormFile File { get; set; }
}
