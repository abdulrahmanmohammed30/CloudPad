using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;

namespace NoteTakingApp.Core.Services;

public class TagManagerService(ITagRepository tagRepository, IUserValidatorService userValidatorService, IMemoryCache cache
, ITagValidatorService tagValidatorService) : ITagManagerService
{
    public async Task DeleteAllAsync(int userId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        cache.Remove($"tags/{userId}");

        await tagRepository.DeleteAllAsync(userId);
    }

    public async Task<TagDto> CreateAsync(int userId, CreateTagDto tagDto)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (tagDto == null)
        {
            throw new TagArgumentNullException("Tag was null");
        }

        if (string.IsNullOrEmpty(tagDto.Name))
        {
            throw new InvalidTagException("Tag name cannot be null or empty");
        }

        if (!string.IsNullOrEmpty(tagDto.Description) && tagDto.Description.Length > 500)
        {
            throw new InvalidTagException("Tag description cannot be more than 500 characters");
        }

        if (await tagValidatorService.ExistsAsync(userId, tagDto.Name))
        {
            throw new DuplicateTagNameException("Tag name already exists");
        }

        var tag = tagDto.ToEntity();
        tag.UserId = userId;

        var createdTag = await tagRepository.CreateAsync(tag);

        cache.Remove($"tags/{userId}");

        return createdTag.ToDto();
    }

    public async Task<TagDto> UpdateAsync(int userId, UpdateTagDto tagDto)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (tagDto == null)
        {
            throw new TagArgumentNullException("Tag was null");
        }

        if (string.IsNullOrEmpty(tagDto.Name))
        {
            throw new ArgumentNullException(nameof(tagDto.Name), "Tag name cannot be null or empty");
        }

        if (!string.IsNullOrEmpty(tagDto.Description) && tagDto.Description.Length > 500)
        {
            throw new InvalidTagException("Tag description cannot be more than 500 characters");
        }

        var existingTag = await tagRepository.GetByIdAsync(userId, tagDto.TagId);

        if (existingTag == null)
        {
            throw new TagNotFoundException("Tag not found.");
        }

        if (existingTag.Name != tagDto.Name && await tagValidatorService.ExistsAsync(userId, tagDto.Name))
        {
            throw new DuplicateTagNameException("Tag name already exists");
        }

        existingTag.Name = tagDto.Name;
        existingTag.Description = tagDto.Description;
        existingTag.UpdatedAt = DateTime.UtcNow;

        var updatedTag = await tagRepository.UpdateAsync(existingTag);

        cache.Remove($"tags/{userId}");

        return updatedTag.ToDto();
    }

    [Obsolete("This method is deprecated.")]
    public async Task<List<TagDto>> UpdateNoteTagsAsync(int userId, Guid noteId, List<int> tagIds)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (noteId == Guid.Empty)
        {
            throw new NoteArgumentNullException("Tag id is invalid");
        }

        var tags = await tagRepository.UpdateNoteTagsAsync(userId, noteId, tagIds);
        return tags.ToDtoList();
    }

    public async Task<bool> DeleteAsync(int userId, int tagId)
    {
        await userValidatorService.EnsureUserValidationAsync(userId);

        if (tagId <= 0)
        {
            throw new InvalidTagIdException("Tag id cannot be 0");
        }

        var tag = await tagRepository.GetByIdAsync(userId, tagId);

        if (tag == null)
        {
            throw new TagNotFoundException("Tag was not found");
        }

        tag.IsDeleted = true;
        await tagRepository.UpdateAsync(tag);

        cache.Remove($"tags/{userId}");

        return true;
    }
}
