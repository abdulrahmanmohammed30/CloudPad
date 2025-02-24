namespace NoteTakingApp.Core.Dtos;

public class ProfileDto
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
    public int? PreferredLanguageId { get; set; }
    public string? PreferredLanguageName { get; set; }
    public List<UserSocialLinkDto>? SocialLinks { get; set; } 
    public int NotesCount { get; set; }
    public int CategoriesCount { get; set; }
    public int TagsCount { get; set; }

}

