using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Core.Dtos;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(50)]
    [Remote("ValidateCategoryName", "Note")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsFavorite { get; set; } = false;
}