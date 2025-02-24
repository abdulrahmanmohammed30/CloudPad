using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
}