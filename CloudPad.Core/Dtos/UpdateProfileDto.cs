using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Attributes.ValidationAttributes;

namespace CloudPad.Core.Dtos;

public class UpdateProfileDto
{
    [Required()]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(3000)]
    [DataType(DataType.MultilineText)]
    public string? Bio { get; set; }
    
    [Range(1, int.MaxValue)]
    public short? CountryId { get; set; }
    
    [Range(1, int.MaxValue)]
    public int? PreferredLanguageId { get; set; }
    
    [DataType(DataType.Date)]
    [BirthDateValidation]
    public DateOnly? BirthDate { get; set; }
}
