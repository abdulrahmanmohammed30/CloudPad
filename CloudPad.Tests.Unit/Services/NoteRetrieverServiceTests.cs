using CloudPad.Core.Entities;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NoteTakingApp.Tests.Unit.Services;

public class NoteRetrieverServiceTests
{
    private readonly INoteRepository _noteRepository = Substitute.For<INoteRepository>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();
    private readonly INoteRetrieverService _sut;
    private const int UserId = 1;

    public NoteRetrieverServiceTests()
    {
        _sut = new NoteRetrieverService(_noteRepository, _userValidatorService);
        _userValidatorService.EnsureUserValidationAsync(Arg.Is(UserId)).Returns(Task.CompletedTask);
    }

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ShouldReturnsNull_WhenNoteIdIsInvalid()
    {
        // Arrange
        var noteId = Guid.Empty;

        // Act
        var result = await _sut.GetByIdAsync(UserId, noteId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnsNull_WhenNoteWasNotFound()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        _noteRepository.GetByIdAsync(UserId, Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(UserId, noteId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnsNoteDto_WhenNoteWasFound()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        var note = new Note()
        {
            NoteId = 1,
            UserId = UserId,
            NoteGuid = Guid.NewGuid(),
            Title = "note title",
            Content = "note content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Tags = [],
            Category = null,
            CategoryId = null,
            Resources = [],
            IsDeleted = false
        };
        _noteRepository.GetByIdAsync(UserId, Arg.Any<Guid>()).Returns(note);

        var expectedResult = note.ToDto();
        // Act
        var result = await _sut.GetByIdAsync(UserId, noteId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    #endregion GetByIdAsync

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ShouldReturnsEmptyList_WhenNoNotesFound()
    {
        // Arrange
        _noteRepository.GetAllAsync(UserId).Returns(new List<Note>());

        // Act
        var result = await _sut.GetAllAsync(UserId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldUseDefaultPagination_WhenNoParametersProvided()
    {
        // Arrange
        var notes = new List<Note>
        {
            new() { NoteId = 1, Title = "Note 1" },
            new() { NoteId = 2, Title = "Note 2" }
        };

        _noteRepository.GetAllAsync(UserId, 1, 20).Returns(notes);

        // Act
        var result = await _sut.GetAllAsync(UserId);

        // Assert
        result.Should().BeEquivalentTo(notes.ToDtoList());
    }


    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(1, 5)]
    public async Task GetAllAsync_ShouldReturnPaginatedNotes_WhenValidParametersProvided(int pageNumber, int pageSize)
    {
        // Arrange
        var allNotes = new List<Note>
        {
            new() { NoteId = 1, Title = "Note 1" },
            new() { NoteId = 2, Title = "Note 2" },
            new() { NoteId = 3, Title = "Note 3" },
            new() { NoteId = 4, Title = "Note 4" },
            new() { NoteId = 5, Title = "Note 5" }
        };

        var paginatedNotes = allNotes
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        _noteRepository.GetAllAsync(UserId, pageNumber, pageSize).Returns(paginatedNotes);

        var expectedNotes = paginatedNotes.ToDtoList();

        // Act
        var result = await _sut.GetAllAsync(UserId, pageNumber, pageSize);

        // Assert
        result.Should().BeEquivalentTo(expectedNotes);
    }

    [Theory]
    [InlineData(0, 20)] // Invalid page number
    [InlineData(1, 0)] // Invalid page size
    [InlineData(-1, 20)] // Negative page number
    [InlineData(1, -5)] // Negative page size
    public async Task GetAllAsync_ShouldNormalizeInvalidParameters_WhenProvided(int pageNumber, int pageSize)
    {
        // Arrange
        var notes = new List<Note>
        {
            new() { NoteId = 1, Title = "Note 1" },
            new() { NoteId = 2, Title = "Note 2" }
        };

        var normalizedPageNumber = pageNumber <= 0 ? 1 : pageNumber;
        var normalizedPageSize = pageSize <= 0 ? 20 : pageSize;

        _noteRepository.GetAllAsync(UserId, normalizedPageNumber, normalizedPageSize).Returns(notes);

        // Act
        var result = await _sut.GetAllAsync(UserId, pageNumber, pageSize);

        // Assert
        result.Should().BeEquivalentTo(notes.ToDtoList());
    }

    #endregion GetAllAsync
}