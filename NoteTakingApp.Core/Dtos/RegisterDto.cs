using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NoteTakingApp.Core.Attributes.Enums;
using NoteTakingApp.Core.Attributes.ValidationAttributes;

namespace NoteTakingApp.Core.Dtos;

public class RegisterDto
{
    [Required]
    [MaxLength(100)]
    // Todo: Trim space 
    public string Name { get; set; }=string.Empty;
    
    [MaxLength(500)]
    [Url]
    [DisplayName("Profile Image Url")]
    // Todo: Trim space 
    public string? ProfileImageUrl { get; set; }
    
    public int? CountryId { get; set; }
    
    [DataType(DataType.Date)]
    [BirthDateValidation]
    public DateOnly? BirthDate { get; set; }

    [Required]
    [DisplayName("UserName")]
    [UserName]
    // Todo: Validate username is unique 
    // Todo: string cleaning: Trim space 
    public string UserName { get; set; } = string.Empty;

    public UserRole UserRole { get; set; } = UserRole.User;

    [Required]
    [PasswordComplexity]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}