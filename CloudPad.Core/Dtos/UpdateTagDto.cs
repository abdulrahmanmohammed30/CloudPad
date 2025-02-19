using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Core.Dtos;

public class UpdateTagDto
{
    public int TagId { get; set; }
    [Required]
    [MaxLength(50)]
    [Remote("ValidateTagName", "Tag", ErrorMessage ="Tag name is in use")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
}

    