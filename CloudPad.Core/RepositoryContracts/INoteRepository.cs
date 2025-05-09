﻿using CloudPad.Core.Entities;
using CloudPad.Core.Enums;

namespace CloudPad.Core.RepositoryContracts
{
    public interface INoteRepository
    {
        Task<Note?> GetByIdAsync(int userId, Guid noteId);

        Task<IEnumerable<Note>> GetByCategoryAsync(int userId, Guid categoryGuid, int pageNumber = 0,
            int pageSize = 20);

        Task<IEnumerable<Note>> GetByTagAsync(int userId, int tagId, int pageNumber = 0, int pageSize = 20);
        Task<IEnumerable<Note>> GetFavoritesAsync(int userId, int pageNumber = 0, int pageSize = 20);
        Task<IEnumerable<Note>> GetPinnedAsync(int userId, int pageNumber = 0, int pageSize = 20);
        Task<IEnumerable<Note>> GetArchivedAsync(int userId, int pageNumber = 0, int pageSize = 20);
        Task<IEnumerable<Note>> GetAllAsync(int userId, int pageNumber = 0, int pageSize = 20);

        Task<IEnumerable<Note>> SearchAsync(int userId, string searchTerm, int pageNumber = 0, int pageSize = 20);

        Task<IEnumerable<Note>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = 0,
            int pageSize = 20);

        Task<IEnumerable<Note>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = 0,
            int pageSize = 20);

        Task<IEnumerable<Note>> FilterAsync(int userId, NoteSearchableColumn searchableColumn, string value,
            int pageNumber = 0, int pageSize = 20);

        Task<IEnumerable<Note>> SortAsync(int userId, NoteSortableColumn noteSortableColumn, bool sortDescending = true,
            int pageNumber = 0, int pageSize = 20);

        Task<Note> CreateAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<Note?> RestoreAsync(int userId, Guid noteId);
        Task DeleteAsync(int userId, Guid noteId);
        Task<bool> ExistsAsync(int userId, Guid noteId);
        Task<IEnumerable<Note>> FilterAsync(int userId, string title, string content, string tag, string category, bool isFavorite,
            bool isPinned, bool isArchived, int pageNumber, int pageSize);
    }
}