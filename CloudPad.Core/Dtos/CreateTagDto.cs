using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Core.Dtos;

public class CreateTagDto
{
    [Required(AllowEmptyStrings =false)]
    [MaxLength(50)]
    [Remote("ValidateTagName", "Tag", ErrorMessage ="Name is already taken.")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
}