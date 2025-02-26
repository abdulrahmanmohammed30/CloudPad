using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CloudPad.Core.Dtos;

public class CreateCategoryDto
{
    [Required]
    //[StringLength(12, MinimumLength = 8, ErrorMessage = "Name must be between 8 and 50 characters")]
    [MaxLength(12, ErrorMessage ="Name cannot be more than 50 characters")]
    [Remote("ValidateCategoryName", "Category", ErrorMessage ="Name is already taken.")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsFavorite { get; set; } = false;
}