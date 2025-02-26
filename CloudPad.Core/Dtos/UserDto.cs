using CloudPad.Core.Attributes.Enums;

namespace CloudPad.Core.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public short? CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Role { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Bio { get; set; }
}