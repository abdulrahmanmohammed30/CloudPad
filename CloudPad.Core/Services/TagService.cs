using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Core.Services;

public class TagService(ITagRepository tagRepository,   
    IUserService userService) : ITagService
{
    private async Task ValidateUserAsync(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

        if (!await userService.ExistsAsync(userId))
        {
            throw new UserNotFoundException($"User with id {userId} doesn't exist");
        }
    }
    
    public async Task<TagDto?> GetByIdAsync(int userId, int id)
    {
        await ValidateUserAsync(userId);
        
        var tag = await tagRepository.GetByIdAsync(userId, id);
        return tag?.ToDto();
    }

    public async Task<IEnumerable<TagDto>> GetAllAsync(int userId)
    {        
        await ValidateUserAsync(userId);
        
        var tags = await tagRepository.GetAllAsync(userId);
        return tags.ToDtoList();
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
        await ValidateUserAsync(userId);
        
        if (string.IsNullOrEmpty(tagDto.Name))
        {
            throw new ArgumentNullException(nameof(tagDto.Name), "Tag name cannot be null or empty");
        }

        if (await ExistsAsync(userId, tagDto.Name))
        {
            throw new DuplicateCategoryNameException("Tag name already exists");
        }
        
        var tag = tagDto.ToEntity();
        var createdTag = await tagRepository.CreateAsync(userId, tag);
        return createdTag.ToDto();
    }

    public async Task<TagDto> UpdateAsync(int userId, UpdateTagDto tagDto)
    {
        await ValidateUserAsync(userId);
        
        if (string.IsNullOrEmpty(tagDto.Name))
        {
            throw new ArgumentNullException(nameof(tagDto.Name), "Tag name cannot be null or empty");
        }

        if (await ExistsAsync(userId, tagDto.Name))
        {
            throw new DuplicateCategoryNameException("Tag name already exists");
        }
        
        var tag = tagDto.ToEntity();
        var updatedTag = await tagRepository.UpdateAsync(userId, tag);
        return updatedTag.ToDto();
    }

    public async Task<List<TagDto>> UpdateNoteTagsAsync(int userId, Guid noteId, List<int> tagIds)
    {
        if (tagIds.Count == 0)
            return [];
        
        var tags = await tagRepository.UpdateNoteTagsAsync(userId, noteId, tagIds);
        return tags.ToDtoList();
    }
}