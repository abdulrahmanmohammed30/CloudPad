using Microsoft.Extensions.Caching.Memory;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class TagService(ITagRepository tagRepository,
    IUserValidationService userValidationService, IMemoryCache cache) : ITagService
{
    public async Task<TagDto?> GetByIdAsync(int userId, int id)
    {
        await userValidationService.EnsureUserValidation(userId);

        var tag = await tagRepository.GetByIdAsync(userId, id);
        return tag?.ToDto();
    }

    public async Task<IEnumerable<TagDto>> GetAllAsync(int userId)
    {
        return await cache.GetOrCreateAsync($"tags/{userId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            await userValidationService.EnsureUserValidation(userId);
            var tags = await tagRepository.GetAllAsync(userId);
            return tags.ToDtoList();
        }) ?? throw new InvalidOperationException();
    }

    public async Task DeleteAllAsync(int userId)
    {
        cache.Remove($"tags/{userId}");

         await tagRepository.DeleteAllAsync(userId);
    }


    public async Task<bool> ExistsAsync(int userId, int id)
    {
        return await tagRepository.ExistsAsync(userId, id);
    }

    public async Task<bool> ExistsAsync(int userId, string name)
    {
        return await tagRepository.ExistsAsync(userId, name);
    }

    public async Task<TagDto> CreateAsync(int userId, CreateTagDto tagDto)
    {
        await userValidationService.EnsureUserValidation(userId);

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

        if (await ExistsAsync(userId, tagDto.Name))
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
        await userValidationService.EnsureUserValidation(userId);

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

        if (existingTag.Name != tagDto.Name && await ExistsAsync(userId, tagDto.Name))
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
        await userValidationService.EnsureUserValidation(userId);

        if (noteId == Guid.Empty)
        {
            throw new TagArgumentNullException("Tag id is invalid");
        }

        if (tagIds == null)
            tagIds = [];

        var tags = await tagRepository.UpdateNoteTagsAsync(userId, noteId, tagIds);
        return tags.ToDtoList();
    }

    public async Task<bool> DeleteAsync(int userId, int tagId)
    {
        await userValidationService.EnsureUserValidation(userId);

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

    public async Task<TagDto?> GetByNameAsync(int userId, string name)
    {
        await userValidationService.EnsureUserValidation(userId);

        var tag = await tagRepository.GetByNameAsync(userId, name);
        return tag?.ToDto();

    }
}