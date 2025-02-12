using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.Mappers;

public static class TagMapper {
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto()
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Description = tag.Description,
        };
    }

    public static Tag ToEntity(this TagDto tagDto)
    {
        return new Tag()
        {
            TagId = tagDto.TagId,
            Name = tagDto.Name,
            Description = tagDto.Description,
            Notes = new List<Note>(),
        };
    }
    
    public static Tag ToEntity(this CreateTagDto tagDto)
    {
        return new Tag()
        {
            Name = tagDto.Name,
            Description = tagDto.Description,
            Notes = new List<Note>(),
        };
    }
    
    public static Tag ToEntity(this UpdateTagDto tagDto)
    {
        return new Tag()
        {
            TagId = tagDto.TagId,
            Name = tagDto.Name,
            Description = tagDto.Description,
            Notes = new List<Note>(),
        };
    }
    public static List<TagDto> ToDtoList(this List<Tag> tags)
    {
        return tags.Select(t => t.ToDto()).ToList();
    }
}