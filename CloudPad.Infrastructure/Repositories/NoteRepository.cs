﻿using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using CloudPad.Core.Entities;
using CloudPad.Core.Enums;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using CloudPad.Core.Expressions;


namespace CloudPad.Infrastructure.Repositories;

public class NoteRepository(AppDbContext context) : INoteRepository
{
    public async Task<Note?> GetById(int userId, Guid noteId)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.NoteGuid == noteId)
            .Include(n => n.Resources)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Note>> GetByCategoryAsync(int userId, Guid categoryGuid, int pageNumber = 0,
        int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.Category != null && n.Category.CategoryGuid == categoryGuid)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> GetByTagAsync(int userId, int tagId, int pageNumber = 0, int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.Tags.Any(t => t.TagId == tagId))
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> GetFavoritesAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.IsFavorite)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> GetPinnedAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.IsPinned)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> GetArchivedAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.IsArchived)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> GetAllAsync(int userId, int pageNumber = 0, int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> SearchAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId &&
                        (n.Title.Contains(searchTerm) || (n.Content != null && n.Content.Contains(searchTerm))))
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> SearchByTitleAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.Title.Contains(searchTerm))
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> SearchByContentAsync(int userId, string searchTerm, int pageNumber = 0,
        int pageSize = 20)
    {
        return await context.Notes
            .Where(n => n.UserId == userId && n.Content != null && n.Content.Contains(searchTerm))
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Note>> FilterAsync(int userId, NoteSearchableColumn searchableColumn, string value,
        int pageNumber = 0, int pageSize = 20)
    {
        Expression<Func<Note, bool>> filter = n => n.UserId == userId;

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

        return await context.Notes
            .Where(filter)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }


    public async Task<IEnumerable<Note>> SortAsync(int userId, NoteSortableColumn column, bool sortDescending = true,
        int pageNumber = 0, int pageSize = 20)
    {
        var query = context.Notes
            .Where(n => n.UserId == userId);

        query = column switch
        {
            NoteSortableColumn.Title => sortDescending
                ? query.OrderByDescending(n => n.Title)
                : query.OrderBy(n => n.Title),
            NoteSortableColumn.CreatedAt => sortDescending
                ? query.OrderByDescending(n => n.CreatedAt)
                : query.OrderBy(n => n.CreatedAt),
            NoteSortableColumn.UpdatedAt => sortDescending
                ? query.OrderByDescending(n => n.UpdatedAt)
                : query.OrderBy(n => n.UpdatedAt),
            _ => query
        };

        return await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Note> CreateAsync(Note note)
    {
        context.Notes.Add(note);
        await context.SaveChangesAsync();
        return note;
    }

    public async Task<Note> UpdateAsync(Note note)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync();
        return note;
    }

    public async Task<Note?> RestoreAsync(int userId, Guid noteId)
    {
        var note = await GetById(userId, noteId);
        if (note != null)
        {
            note.IsDeleted = false;
            await context.SaveChangesAsync();
        }

        return note;
    }

    //public async Task<bool> DeleteAsync(int userId, Guid noteId)
    //{
    //    var note = await GetById(userId, noteId);
    //    if (note != null)
    //    {
    //        note.IsDeleted = true;
    //        await _context.SaveChangesAsync();
    //        return true;
    //    }
    //    return false;
    //}

    public async Task DeleteAsync(int userId, Guid noteId)
    {
        var note = await GetById(userId, noteId);
        note!.IsDeleted = true;
        context.Notes.Update(note);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int userId, Guid noteId)
    {
        return await context.Notes
            .AnyAsync(n => n.UserId == userId && n.NoteGuid == noteId && !n.IsDeleted);
    }


    public async Task<IEnumerable<Note>> FilterAsync(int userId, string title, string content, string tag,
        string category, bool isFavorite, bool isPinned, bool isArchived, int pageNumber, int pageSize)
    {
        var query = context.Notes.Where(n =>
                n.Title.Contains(title) && (n.Content == null || n.Content.Contains(content)) && n.UserId == userId)
            .Skip(pageNumber * pageSize).Take(pageSize);

        // if the content is not null, filter by content 
        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(n => n.Tags.Any(t => t.Name.ToLower().Equals(tag.ToLower())));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(n => n.Category != null && n.Category.Name.ToLower().Equals(category.ToLower()));
        }

        if (isFavorite)
        {
            query = query.Where(n => n.IsFavorite);
        }

        if (isPinned)
        {
            query = query.Where(n => n.IsPinned);
        }

        if (isArchived)
        {
            query = query.Where(n => n.IsArchived);
        }

        return await query.ToListAsync();
    }
}

