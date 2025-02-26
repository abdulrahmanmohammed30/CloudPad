using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;

namespace CloudPad.Core.Mappers;

public static class ResourceMapper
{
    public static ResourceDto ToDto(this Resource resource)
    {
        return new ResourceDto()
        {
            ResourceId = resource.ResourceId,
            DisplayName = resource.DisplayName,
            FilePath = resource.FilePath,
            Description = resource.Description,
            Size = resource.Size,
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

    public static List<ResourceDto> ToDtoList (this List<Resource> resources)
    {
        return resources.Select(resource => resource.ToDto()).ToList();
    }
}