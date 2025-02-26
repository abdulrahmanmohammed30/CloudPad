using CloudPad.Core.Entities;
using CloudPad.Core.Dtos;

namespace CloudPad.Core.RepositoryContracts;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int userId, int tagId);
    Task<List<Tag>> GetAllAsync(int userId);
    Task<bool> ExistsAsync(int userId, int tagId);
    Task<bool> ExistsAsync(int userId, string tagName);
    Task<Tag> CreateAsync(Tag tag);
    Task<Tag> UpdateAsync(Tag tag);
    Task<List<Tag>> UpdateNoteTagsAsync(int userId, Guid noteId, List<int> tagIds);
    Task DeleteAsync(int userId, int tagId);
    Task<Tag?> GetByNameAsync(int userId, string name);
    Task DeleteAllAsync(int userId);
}