using CloudPad.Core.Entities;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CloudPad.Infrastructure.Repositories
{
    public class ResourceRepository(AppDbContext context) : IResourceRepository
    {
        public async Task<Resource> CreateAsync(Resource resource)
        {
            context.Resources.Add(resource);
            var d = context.ChangeTracker.DebugView.ShortView;
            await context.SaveChangesAsync();
            return resource;
        }

        public async Task<List<Resource>> GetAllAsync(int userId, Guid noteId)
        {
            return await context.Resources.Where(r => r.Note.NoteGuid == noteId && r.Note.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int userId, Guid noteId, Guid resourceId)
        {
            var existingResource = await GetByIdAsync(userId, noteId, resourceId);
            existingResource!.IsDeleted = true;
            context.Update(existingResource);
            await context.SaveChangesAsync();
        }

        public async Task<Resource?> GetByIdAsync(int userId, Guid noteId, Guid resourceId)
        {
            return await context.Resources
                .Where(r => r.ResourceId == resourceId && r.Note.NoteGuid == noteId && r.Note.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<Resource> UpdateAsync(Resource resource)
        {
            context.Resources.Update(resource);
            await context.SaveChangesAsync();
            return resource;
        }

        public async Task<bool> ExistsAsync(int userId, Guid noteId, Guid resourceId)
        {
            return await context.Resources.AnyAsync(r =>
                r.ResourceId == resourceId && r.Note.NoteGuid == noteId && r.Note.UserId == userId);
        }
    }
}