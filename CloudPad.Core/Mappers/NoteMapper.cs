using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;

namespace NoteTakingApp.Core.Mappers;

public static class NoteMapper
{
    public static NoteDto ToDto(this Note note)
    {
        return new NoteDto()
        {
            NoteId = note.NoteGuid,
            Title = note.Title,
            Content = note.Content,
            Category = note.Category?.ToDto(),
            Tags = note.Tags.Select(t => t.ToDto()).ToList(),
            IsFavorite = note.IsFavorite,
            IsArchived = note.IsArchived,
            IsPinned = note.IsPinned,
            Resources = note.Resources.Select(r => r.ToDto()).ToList()
        };
    }
    
    public static List<NoteDto> ToDtoList(this IEnumerable<Note> notes)
    {
        return notes.Select(n => n.ToDto()).ToList();
    }

    // Todo: Enrich with IsDeleted, CreatedAt, UpdatedAt, CategoryId, IsDeleted, UserId 
    public static Note ToEntity(this NoteDto noteDto)
    {
        return new Note()
        {
            NoteGuid = noteDto.NoteId,
            IsArchived = noteDto.IsArchived,
            IsPinned = noteDto.IsPinned,
            Title = noteDto.Title,
            Content = noteDto.Content,
            IsFavorite = noteDto.IsFavorite,
            Resources = noteDto.Resources.Select(r => r.ToEntity()).ToList()
        };
    }
    
    
}