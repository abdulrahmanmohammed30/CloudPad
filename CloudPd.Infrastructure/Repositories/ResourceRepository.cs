using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;

namespace NoteTakingApp.Infrastructure.Repositories
{
    public class ResourceRepository(AppDbContext context): IResourceRepository
    {
        public async Task<Resource> CreateAsync(Guid noteId, Resource resource)
        {
            resource.CreatedAt= DateTime.UtcNow;
            resource.UpdatedAt = DateTime.UtcNow;

            var note = await context.Notes.AsTracking()
                .Where(n => n.NoteGuid == noteId)
                .SingleOrDefaultAsync();

            resource.Note= note;
            resource.NoteId = note.NoteId;

            context.Resources.Add(resource);

            await context.SaveChangesAsync();
            return resource;
        }

        public async Task<List<Resource>> GetAllAsync(Guid noteId)
        {
            return await context.Resources.Where(r=>r.Note.NoteGuid == noteId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid resourceId)
        {
            var existingResource = await GetByIdAsync(resourceId);
            existingResource.IsDeleted = true;
            context.Update(existingResource);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Resource?> GetByIdAsync(Guid resourceId)
        {
            return await context.Resources.FirstOrDefaultAsync(r => r.ResourceId == resourceId);
        }

        public async Task<bool> Exists(Guid resourceId)
        {
            return await context.Resources.AnyAsync(r=>r.ResourceId == resourceId);
        }
    }
}
