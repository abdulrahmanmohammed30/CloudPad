namespace NoteTakingApp.Core.Domains;

public class ApplicationUserWithRole
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PasswordHash { get; set; }
    public string? SecurityStamp { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ProfileImageUrl { get; set; }

    public short? CountryId { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? CountryName { get; set; }

    public string? RoleName { get; set; } 
}
