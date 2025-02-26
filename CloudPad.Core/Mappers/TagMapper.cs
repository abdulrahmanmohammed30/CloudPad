using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;

namespace CloudPad.Core.Mappers;

public static class TagMapper {
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto()
        {
            Id = tag.TagId,
            Name = tag.Name,
            Description = tag.Description,
        };
    }

    public static Tag ToEntity(this TagDto tagDto)
    {
        return new Tag()
        {
            TagId = tagDto.Id,
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
    
    public static UpdateTagDto ToUpdateDto(this Tag tag)
    {
        return new UpdateTagDto()
        {
          TagId = tag.TagId,
          Name = tag.Name,
          Description = tag.Description,
        };
    }
    public static List<UpdateTagDto> ToUpdateDtoList(this List<Tag> tags)
    {
        return tags.Select(t => t.ToUpdateDto()).ToList();
    }
}