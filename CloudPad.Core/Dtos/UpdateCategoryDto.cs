using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Core.Dtos;

public class UpdateCategoryDto
{
    public Guid CategoryId { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage ="Category characters cannot be more than 50")]
    [Remote("ValidateExistingCategoryName", "Category", ErrorMessage ="Category Name is in use.", AdditionalFields = nameof(CategoryId))]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsFavorite { get; set; } = false;
}
