using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Enums;
using NoteTakingApp.Core.Expressions;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Infrastructure.Context;


namespace NoteTakingApp.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Note?> GetById(int userId, Guid noteId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.NoteGuid == noteId && !n.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Note>> GetByCategoryAsync(int userId, Guid categoryGuid, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.Category != null && n.Category.CategoryGuid == categoryGuid && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetByTagAsync(int userId, int tagId, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.Tags.Any(t => t.TagId == tagId) && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetFavoritesAsync(int userId, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.IsFavorite && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetPinnedAsync(int userId, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.IsPinned && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetArchivedAsync(int userId, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.IsArchived && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetAllAsync(int userId, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> SearchAsync(int userId, string searchTerm, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId &&
                            (n.Title.Contains(searchTerm) || n.Content.Contains(searchTerm)) &&
                            !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.Title.Contains(searchTerm) && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = 0, int pageSize = 20)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId && n.Content.Contains(searchTerm) && !n.IsDeleted)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> FilterAsync(int userId, NoteSearchableColumn searchableColumn, string value, int pageNumber = 0, int pageSize = 20)
        {
            Expression<Func<Note, bool>> filter = n => n.UserId == userId && !n.IsDeleted;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var parameter = Expression.Parameter(typeof(Note), "n");
                Expression property = Expression.Property(parameter, searchableColumn.ToString());
                Expression condition = Expression.Call(
                    property,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                    Expression.Constant(value, typeof(string))
                );

                var lambda = Expression.Lambda<Func<Note, bool>>(condition, parameter);
                filter = filter.And<Note>(lambda); // Using a helper method to combine expressions
            }

            return await _context.Notes
                .Where(filter)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        
        public async Task<IEnumerable<Note>> SortAsync(int userId, NoteSortableColumn column, bool sortDescending = true, int pageNumber = 0, int pageSize = 20)
        {
            var query = _context.Notes
                .Where(n => n.UserId == userId && !n.IsDeleted);

            query = column switch
            {
                NoteSortableColumn.Title => sortDescending ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title),
                NoteSortableColumn.CreatedAt => sortDescending ? query.OrderByDescending(n => n.CreatedAt) : query.OrderBy(n => n.CreatedAt),
                NoteSortableColumn.UpdatedAt => sortDescending ? query.OrderByDescending(n => n.UpdatedAt) : query.OrderBy(n => n.UpdatedAt),
                _ => query
            };

            return await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Note> AddAsync(int userId, Note note)
        {
            note.UserId = userId;
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task<Note> UpdateAsync(Note note)
        {
            note.UpdatedAt = DateTime.UtcNow;

            _context.Notes.Update(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task<Note?> RestoreAsync(int userId, Guid noteId)
        {
            var note = await GetById(userId, noteId);
            if (note != null)
            {
                note.IsDeleted = false;
                await _context.SaveChangesAsync();
            }

            return note;
        }

        public async Task<bool> DeleteAsync(int userId, Guid noteId)
        {
            var note = await GetById(userId, noteId);
            if (note != null)
            {
                note.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Exists(int userId, Guid noteId)
        {
            return await _context.Notes
                .AnyAsync(n => n.UserId == userId && n.NoteGuid == noteId && !n.IsDeleted);
        }
    }
}