using CloudPad.Core.Dtos;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.Domains;

namespace CloudPad.Core.Mappers;

public static class ProfileMapper
{
    public static ProfileDto ToProfileDto(this ApplicationUser user)
    {
        return new ProfileDto()
        {
            BirthDate = user.BirthDate,
            CountryId = (short?)user.CountryId,
            Name = user.Name.Trim(),
            Username = user.UserName.Trim(),
            Email=user.Email.Trim(), 
            CountryName = user.Country?.EnglishName,
            Bio = user.Bio,
            ProfileImageUrl = user.ProfileImageUrl,
            CreatedAt = user.CreatedAt,
            PreferredLanguageId = user.PreferredLanguageId,
            PreferredLanguageName = user.PreferredLanguage?.Name,
            Role = user.Role?.Name,
            SocialLinks = user.SocialLinks.Select(s=>new UserSocialLinkDto()
            {
                 UserSocialLinkId = s.UserSocialLinkId,
                 UserSocialLinkUrl = s.Url
            }).ToList(),
            CategoriesCount = user.Categories.Count,
            NotesCount = user.Notes.Count,
            TagsCount = user.Tags.Count,
            Id = user.Id
        };
    }
}