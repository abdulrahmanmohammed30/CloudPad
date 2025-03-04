using System.Globalization;
using System.Security.Cryptography;
using CloudPad.Core.Entities;
using CloudPad.Core.Enums;
using CloudPad.Core.Exceptions;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using FluentAssertions;
using NoteTakingApp.Core.Enums;
using NSubstitute;

namespace NoteTakingApp.Tests.Unit.Services;

public class NoteFilterServiceTests
{
    private readonly INoteRepository _noteRepository = Substitute.For<INoteRepository>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();
    private readonly INoteFilterService _sut;
    private const int UserId = 1;


    public NoteFilterServiceTests()
    {
        _sut = new NoteFilterService(_noteRepository, _userValidatorService);
    }

    #region FilterByAsync

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task FilterByAsync_ShouldThrowException_WhenColumnIsNullOrEmpty(string column)
    {
        // Arrange
        const string value = "";
        int pageNumber = 1;
        int pageSize = 15;

        // result
        Func<Task> result = async () => await _sut.FilterByAsync(UserId, column, value, pageNumber, pageSize);

        // Assert
        await result.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Search column cannot be null or empty (Parameter 'column')");
    }

    [Fact]
    public async Task FilterByAsync_ShouldThrowException_WhenColumnIsNotSearchable()
    {
        // Arrange
        string column = "invalid-column";
        string value = "value";
        int pageNumber = 1;
        int pageSize = 15;

        // result
        Func<Task> result = async () => await _sut.FilterByAsync(UserId, column, value, pageNumber, pageSize);

        // Assert
        await result.Should().ThrowAsync<InvalidSearchColumnException>().WithMessage("column is not searchable");
    }

    [Fact]
    public async Task FilterByAsync_ShouldThrowException_WhenSearchValueIsInvalid()
    {
        // Arrange
        string column = "CreatedAt";
        string value = "invalid-value";
        int pageNumber = 1;
        int pageSize = 15;

        // result
        var result = async () => await _sut.FilterByAsync(UserId, column, value, pageNumber, pageSize);

        // Assert
        await result.Should().ThrowAsync<InvalidSearchValueException>()
            .WithMessage($"value {value} is not assignable to column {column}");
    }


    [Fact]
    public async Task FilterByAsync_ShouldReturnNonFilteredNotes_WhenValueIsEmptyOrNull()
    {
        // Arrange
        string column = "IsArchived";
        string value = "";
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };

        _noteRepository.GetAllAsync(UserId, pageNumber, pageSize).Returns(notes);

        // result
        var result = await _sut.FilterByAsync(UserId, column, value, pageNumber, pageSize);

        // Assert
        result.Should().BeEquivalentTo(notes.ToDtoList());
    }


    [Fact]
    public async Task FilterByAsync_ShouldReturnFilteredNotes_WhenInvoked()
    {
        // Arrange
        string column = "IsArchived";
        string value = "true";
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };

        _noteRepository.FilterAsync(UserId, Arg.Any<NoteSearchableColumn>(),
            value, pageNumber, pageSize).Returns(notes);

        // result
        var result = await _sut.FilterByAsync(UserId, column, value, pageNumber, pageSize);

        // Assert
        result.Should().BeEquivalentTo(notes.ToDtoList());
    }

    #endregion

    #region FilterAsync

    public async Task FilterAsync_ShouldReturnFilteredNotes_WhenInvoked()
    {
        // Arrange 
        string title = "title";
        string content = "content";
        string tag = "tag";
        string category = "category";
        bool isFavorite = true;
        bool isPinned = true;
        bool isArchived = true;
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
        
        _noteRepository.FilterAsync(UserId, title,
                content, tag, category, isFavorite, isPinned,
                isArchived, pageNumber, pageSize)
            .Returns(notes);

        var expectedResult = notes.ToDtoList();
        
        // Act 
        var result = _sut.FilterAsync(
            UserId, title,
            content, tag, category, isFavorite, isPinned,
            isArchived, pageNumber, pageSize);

        // Assert 
        result.Should().BeEquivalentTo(expectedResult);
    }
    #endregion
    
    #region SearchAsync 
    
    [Fact]
    public async Task SearchAsync_ShouldReturnNonFilteredNotes_WhenSearchTermIsEmpty()
    {
        // Arrange 
        string searchTerm = "";
        SearchFields searchFields = SearchFields.Title;
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
        
        _noteRepository.GetAllAsync(UserId, 
                pageNumber, pageSize)
            .Returns(notes);

        var expectedResult = notes.ToDtoList();
        
        // Act 
        var result = await _sut.SearchAsync(
            UserId, searchTerm, searchFields, pageNumber, pageSize);

        // Assert 
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    
    [Fact]
    public async Task SearchAsync_ShouldReturnFilteredNotes_WhenInvoked()
    {
        // Arrange 
        string searchTerm = "searchTerm";
        SearchFields searchFields = SearchFields.Title;
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
        
        _noteRepository.SearchAsync(UserId, searchTerm, 
                pageNumber, pageSize)
            .Returns(notes);

        var expectedResult = notes.ToDtoList();
        
        // Act 
        var result = await _sut.SearchAsync(
            UserId, searchTerm, searchFields, pageNumber, pageSize);

        // Assert 
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task SearchByTitleAsync_ShouldCallSearchAsyncWithTitle_WhenInvoked()
    {
        // Arrange 
        string searchTerm = "searchTerm";
        SearchFields searchFields = SearchFields.Title;
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
        
        _noteRepository.SearchAsync(UserId, searchTerm, 
                pageNumber, pageSize)
            .Returns(notes);
        
        var expectedResult = notes.ToDtoList();
        
        // Act
        var result = await _sut.SearchByTitleAsync
            (UserId, searchTerm, pageNumber, pageSize);

        // Assert
        await _noteRepository.Received(1).SearchAsync(UserId, searchTerm,
            pageNumber, pageSize);
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    
    [Fact]
    public async Task SearchByContentAsync_ShouldCallSearchAsyncWithContent_WhenInvoked()
    {
        // Arrange 
        string searchTerm = "searchTerm";
        SearchFields searchFields = SearchFields.Title;
        int pageNumber = 1;
        int pageSize = 15;

        var notes = new List<Note>()
        {
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Note
            {
                NoteId = RandomNumberGenerator.GetInt32(300, 500),
                NoteGuid = Guid.NewGuid(),
                Title = "Title",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        };
        
        _noteRepository.SearchAsync(UserId, searchTerm, 
                pageNumber, pageSize)
            .Returns(notes);
        
        var expectedResult = notes.ToDtoList();
        
        // Act
        var result = await _sut.SearchByContentAsync
            (UserId, searchTerm, pageNumber, pageSize);

        // Assert
        await _noteRepository.Received(1).SearchAsync(UserId, searchTerm,
            pageNumber, pageSize);
        result.Should().BeEquivalentTo(expectedResult);
    }

    
    #endregion 
}