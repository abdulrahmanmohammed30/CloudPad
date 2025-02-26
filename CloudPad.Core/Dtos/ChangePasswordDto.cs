using System.ComponentModel.DataAnnotations;

namespace CloudPad.Core.Dtos;

public class ChangePasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; }
}