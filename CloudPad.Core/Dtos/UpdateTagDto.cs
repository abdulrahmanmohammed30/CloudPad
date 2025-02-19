using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Core.Dtos;

public class UpdateTagDto
{
    public int TagId { get; set; }
    [Required]
    [MaxLength(50)]
    [Remote("ValidateExistingTagName", "Tag", ErrorMessage = "Tag name is in use", AdditionalFields = nameof(TagId))]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
