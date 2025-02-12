using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;

namespace NoteTakingApp.Infrastructure.Repositories;

public class TagRepository(AppDbContext context) : ITagRepository
{
    public async Task<Tag?> GetByIdAsync(int userId, int tagId)
    {
        return await context.Tags
            .Where(t => t.UserId == userId && t.TagId == tagId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Tag>> GetAllAsync(int userId)
    {
        return await context.Tags
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int userId, int tagId)
    {
        return await context.Tags
            .AnyAsync(t => t.UserId == userId && t.TagId == tagId);
    }

    public async Task<bool> ExistsAsync(int userId, string tagName)
    {
        return await context.Tags
            .AnyAsync(t => t.UserId == userId && t.Name == tagName);
    }
    
    public async Task<Tag> CreateAsync(int userId, Tag tag)
    {
        tag.UserId = userId;
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateAsync(int userId, Tag tag)
    {
        var existingTag = await context.Tags
            .Where(t => t.UserId == userId && t.TagId == tag.TagId)
            .FirstOrDefaultAsync();

        if (existingTag == null)
        {
            throw new TagNotFoundException("Tag not found.");
        }

        existingTag.Name = tag.Name;
        existingTag.Description = tag.Description;
        existingTag.UpdatedAt = DateTime.UtcNow;

        context.Tags.Update(existingTag);
        await context.SaveChangesAsync();
        return existingTag;
    }

   public async Task<List<Tag>> UpdateNoteTagsAsync(int userId, Guid noteId, List<int> tagIds)
    {
       var note =await context.Notes.AsTracking().Include(n=>n.Tags).FirstOrDefaultAsync(n => n.NoteGuid == noteId);
       
       var existingTags = await context.Tags
           .Where(ut => ut.UserId == userId && tagIds.Any(t=>t == ut.TagId))
           .ToListAsync();
       
       if (existingTags.Count != tagIds.Count)
           throw new ArgumentException("Some tags do not exist.");
       
       note.Tags.RemoveAll(nt => tagIds.All(t => t != nt.TagId));

       foreach (var tag in existingTags)
       {
           if (note.Tags.All(n => n.TagId != tag.TagId))
           {
               note.Tags.Add(tag);
           }
       }
       
       await context.SaveChangesAsync();
       return note.Tags.ToList();
    }
}