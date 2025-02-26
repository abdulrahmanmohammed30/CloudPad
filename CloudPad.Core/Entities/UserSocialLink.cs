using CloudPad.Core.Entities.Domains;

namespace CloudPad.Core.Entities;

public class UserSocialLink
{
    public int UserSocialLinkId { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    
    public string Url { get; set; } = string.Empty; // Full URL to the profile
}

