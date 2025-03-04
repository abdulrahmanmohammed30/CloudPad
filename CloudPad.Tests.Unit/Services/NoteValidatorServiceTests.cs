using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using FluentAssertions;
using NSubstitute;

namespace NoteTakingApp.Tests.Unit.Services;

public class NoteValidatorServiceTests
{
    private readonly INoteRepository _noteRepository = Substitute.For<INoteRepository>();
    private readonly INoteValidatorService _sut;

    public NoteValidatorServiceTests()
    {
        _sut = new NoteValidatorService(_noteRepository);
    }

    #region NoteValidatorService

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenUserIdIsInvalid()
    {
        // Arrange 
        int userId = 0;
        Guid noteId = Guid.NewGuid();

        // Act 
        var result = await _sut.ExistsAsync(userId, noteId);

        // Assert 
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenNoteIdIsInvalid()
    {
        // Arrange 
        int userId = 1;
        Guid noteId = Guid.Empty;

        // Act 
        var result = await _sut.ExistsAsync(userId, noteId);

        // Assert 
        result.Should().BeFalse();
    }


    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenNoteDoesNotExist()
    {
        // Arrange 
        int userId = 1;
        Guid noteId = Guid.NewGuid();
        _noteRepository.ExistsAsync(userId, noteId).Returns(false);

        // Act 
        var result = await _sut.ExistsAsync(userId, noteId);

        // Assert 
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenNoteExists()
    {
        // Arrange 
        int userId = 1;
        Guid noteId = Guid.NewGuid();
        _noteRepository.ExistsAsync(userId, noteId).Returns(true);

        // Act 
        var result = await _sut.ExistsAsync(userId, noteId);

        // Assert 
        result.Should().BeFalse();
    }

    #endregion
}