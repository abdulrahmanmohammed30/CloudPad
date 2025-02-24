using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class ConfirmEmailChangeDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int UserId { get; set;  }
    
    [Required] 
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Token { get; set; }
}
