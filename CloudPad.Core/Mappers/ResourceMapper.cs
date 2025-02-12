using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.Mappers;

public static class ResourceMapper
{
    public static ResourceDto ToDto(this Resource resourceDto)
    {
        return new ResourceDto()
        {
            ResourceId = resourceDto.ResourceId,
            DisplayName = resourceDto.DisplayName,
            FilePath = resourceDto.FilePath,
            Description = resourceDto.Description,
            Size = resourceDto.Size,
        };
    }

    public static Resource ToEntity(this ResourceDto resourceDto)
    {
        return new Resource()
        {
            ResourceId = resourceDto.ResourceId,
            DisplayName = resourceDto.DisplayName,
            FilePath = resourceDto.FilePath,
            Description = resourceDto.Description,
            Size = resourceDto.Size,
        };
    }
}