using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using NoteTakingApp.Core.Domains;

namespace NoteTakingApp.Core.Entities.Domains;

public class ApplicationUser : IdentityUser<int>
{
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string? ProfileImageUrl { get; set; }

    [ForeignKey(nameof(Country))] public short? CountryId { get; set; }
    public Country? Country { get; set; }

    public DateOnly? BirthDate { get; set; }

    public List<Note> Notes { get; set; } = [];

    public List<Category> Categories { get; set; } = [];

    public List<Tag> Tags { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string? Bio { get; set; }
    
    public int? PreferredLanguageId { get; set; }
    
    public Language? PreferredLanguage { get; set; }

    public List<UserSocialLink>? SocialLinks { get; set; }
    
    [NotMapped]
    public ApplicationRole? Role { get; set; }
    
    [NotMapped]
    public int NotesCount { get; set; }
    
    [NotMapped]
    public int CategoriesCount { get; set; }
    
    [NotMapped]
    public int TagsCount { get; set; }
}