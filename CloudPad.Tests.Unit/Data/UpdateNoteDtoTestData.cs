using System.Collections;
using CloudPad.Core.Dtos;

namespace NoteTakingApp.Tests.Unit.Data;

public class UpdateNoteDtoTestData : IEnumerable<object[]>
{
    public static IEnumerable<UpdateNoteDto> GetTestData()
    {
        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Valid Complete Note",
            Content = "This is a complete note with all fields",
            CategoryId = Guid.NewGuid(),
            Tags = new List<int> { 1, 2, 3 },
            IsPinned = true,
            IsArchived = false,
            IsFavorite = true
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Minimal Valid Note",
            Content = null,
            CategoryId = null,
            Tags = null,
            IsPinned = false,
            IsArchived = false,
            IsFavorite = false
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Note with Maximum Title Length".PadRight(150, '*'),
            Content = "Testing maximum title length scenario",
            CategoryId = Guid.NewGuid(),
            Tags = new List<int> { 1 }
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "A", // Minimum length title
            Content = "",
            Tags = new List<int>()
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Note with Special Characters !@#$%^&*()",
            Content = "Testing special characters in title"
        };
        
        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Note with Large Content",
            Content = new string('x', 10000),
            Tags = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Unicode Title 你好",
            Content = "Testing unicode characters 你好世界"
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Archived and Pinned Note",
            IsArchived = true,
            IsPinned = true
        };

        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "All Flags True",
            IsArchived = true,
            IsPinned = true,
            IsFavorite = true
        };
        
        yield return new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Title with Whitespace",
            Content = "Content with whitespace",
            CategoryId = Guid.NewGuid()
        };
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var item in GetTestData())
        {
            yield return new object[] { item };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}