using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Attributes.ValidationAttributes;

namespace NoteTakingApp.Core.Dtos;

public class RegisterDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }=string.Empty;

    //[MaxLength(500)]
    //[Url]
    //[DisplayName("Profile Image Url")]
    //public string? ProfileImageUrl { get; set; }

    [Required]
    [DataType(DataType.Upload)]
    public IFormFile ImageFile { get; set; }
    
    public int? CountryId { get; set; }
    
    [DataType(DataType.Date)]
    [BirthDateValidation]
    public DateOnly? BirthDate { get; set; }

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;


    [Required]
    [DisplayName("Username")]
    //[UserName]
    [Remote("ValidateUsername", "Account", ErrorMessage = "Username is already taken.")]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [PasswordComplexity]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}