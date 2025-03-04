using CloudPad.Core.Entities;
using CloudPad.Core.Enums;
using CloudPad.Core.Exceptions;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using FluentAssertions;
using NSubstitute;

namespace NoteTakingApp.Tests.Unit.Services;

public class NoteSorterServiceTests
{
    private readonly INoteSorterService _sut;
    private readonly INoteRepository _noteRepository = Substitute.For<INoteRepository>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();
    private const int UserId = 1;

    public NoteSorterServiceTests()
    {
        _sut = new NoteSorterService(_noteRepository, _userValidatorService);
        _userValidatorService.EnsureUserValidationAsync(Arg.Is<int>(x => x == UserId)).Returns(Task.FromResult(true));
    }
    
    public static IEnumerable<object[]> GetTestCases()
    {
        yield return new object[] { "Title", true, 1, 20 }; // Happy path
        yield return new object[] { "Title", false, 1, 20 }; // Different sort order
        yield return new object[] { "Title", true, 2, 20 }; // Different page
        yield return new object[] { "Title", true, 1, 10 }; // Different page size
        yield return new object[] { "Title", true, 0, 20 }; // Invalid page number
        yield return new object[] { "Title", true, 1, 0 }; // Invalid page size
        yield return new object[] { "Title", true, -1, 20 }; // Negative page number
        yield return new object[] { "Title", true, 1, -1 }; // Negative page size
    }
    
    #region SortAsync

    [Fact]
    public async Task SortAsync_ShouldThrowException_WhenColumnIsNullOrWhiteSpace()
    {
        //Arrange
        var userId = 1;
        var column = string.Empty;
        var sortDescending = true;
        var pageNumber = 1;
        var pageSize = 20;

        // Act
        var result = async () =>  await _sut.SortAsync(userId, column, sortDescending, pageNumber, pageSize);

        // Assert
        await result.Should().ThrowAsync<ArgumentNullException>("searchable column cannot be empty");
    }
    
    [Fact]
    public async Task SortAsync_ShouldThrowException_WhenColumnIsNotSortable()
    {
        //Arrange
        var userId = 1;
        var column = "NotSortableColumn";
        var sortDescending = true;
        var pageNumber = 1;
        var pageSize = 20;

        // Act
        var result = async () =>
            await _sut.SortAsync(userId, column, sortDescending, pageNumber, pageSize);

        // Assert
        await result.Should().ThrowAsync<NoteColumnNotSortable>("Not sortable column");
    }
    
    [Theory]
    [MemberData(nameof(GetTestCases))]
    public async Task SortAsync_ShouldReturnSortedNotes_WhenInvoked(string column, bool sortDescending, 
        int pageNumber, int pageSize)
    {
        // Arrange
        var notes = new List<Note>
        {
            new() {NoteId = 1, Title = "Note 1"},
            new() {NoteId = 2, Title = "Note 2"},
            new() {NoteId = 3, Title = "Note 3"}
        };
        _noteRepository.SortAsync(UserId, NoteSortableColumn.Title, sortDescending, Arg.Any<int>(), Arg.Any<int>())
            .Returns(notes);
    
        // Act
        var result = await _sut.SortAsync(UserId, column, sortDescending, pageNumber, pageSize);
    
        // Assert
        result.Should().BeEquivalentTo(notes.ToDtoList());
    }
    
    #endregion SortAsync
}       