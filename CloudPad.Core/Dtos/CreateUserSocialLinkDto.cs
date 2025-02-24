using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class CreateUserSocialLinkDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
    
    [Required]
    [Url]
    [DataType(DataType.Url)]
    public string UserSocialLinkUrl { get; set; } = string.Empty; 
}