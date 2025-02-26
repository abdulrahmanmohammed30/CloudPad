using System.Diagnostics.CodeAnalysis;

namespace CloudPad.Core.Dtos;

public class UserSocialLinkDto
{
    public int UserSocialLinkId { get; set; }
    public string UserSocialLinkUrl { get; set; } = string.Empty; 
}