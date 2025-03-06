using System.Security.Cryptography;
using AutoFixture;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;
using CloudPad.Core.Exceptions;
using CloudPad.Core.Mappers;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using FluentAssertions;
using NoteTakingApp.Tests.Unit.Data;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NoteTakingApp.Tests.Unit.Services;

public class NoteManagerServiceTests
{
    private readonly INoteManagerService _sut;
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly INoteRepository _noteRepository = Substitute.For<INoteRepository>();
    private readonly ITagManagerService _tagManagerService = Substitute.For<ITagManagerService>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();

    private readonly IFixture _fixture;
    private const int UserId = 1;

    public NoteManagerServiceTests()
    {
        _sut = new NoteManagerService(_categoryRepository, _noteRepository, _tagManagerService,
            _userValidatorService);
        _fixture = new Fixture();

        _userValidatorService.EnsureUserValidationAsync(Arg.Is<int>(x => x == UserId)).Returns(Task.FromResult(true));
    }

    #region AddAsync

    [Theory]
    [ClassData(typeof(CreateNoteDtoTestData))]
    public async Task AddAsync_ShouldAddNote_WhenInvoked(CreateNoteDto createNoteDto)
    {
        // Arrange 
        Category? category = null;
        if (createNoteDto.CategoryId.HasValue && createNoteDto.CategoryId != Guid.Empty)
        {
            category = new Category()
            {
                UserId = UserId,
                CategoryId = 1,
                CategoryGuid = Guid.NewGuid()
            };
        }

        _categoryRepository.GetByIdAsync(Arg.Is(UserId), Arg.Any<Guid>()).Returns(category);

        var noteId = RandomNumberGenerator.GetInt32(1, 500);
        var noteGuid = Guid.NewGuid();
        _noteRepository.CreateAsync(Arg.Any<Note>()).Returns(callbackInfo =>
        {
            var createdNote = callbackInfo.Arg<Note>();
            createdNote.NoteId = noteId;
            createdNote.NoteGuid = noteGuid;
            return createdNote;
        });

        var tagDtos = createNoteDto.Tags == null
            ? []
            : createNoteDto.Tags.Select(t => new TagDto()
            {
                Id = t
            }).ToList();

        _tagManagerService.UpdateNoteTagsAsync(Arg.Is(UserId), Arg.Is<Guid>(x => x == noteGuid),
                Arg.Any<List<int>>())
            .Returns(tagDtos);

        var expectedResult = createNoteDto.ToDto();
        expectedResult.Id = noteGuid;
        expectedResult.Tags = tagDtos;
        expectedResult.Category = category?.ToDto();

        // Act
        var result = await _sut.AddAsync(UserId, createNoteDto);

        // Assert 
        if (createNoteDto.CategoryId.HasValue)
        {
            await _categoryRepository.Received(1).GetByIdAsync(Arg.Is(UserId), Arg.Any<Guid>());
        }

        await _noteRepository.Received(1).CreateAsync(Arg.Any<Note>());
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenNoteDataIsInvalid()
    {
        // Arrange 
        // Invalidates a required field i.e. note title
        CreateNoteDto createNoteDto = new CreateNoteDto
        {
            Title = "",
            Content =
                "Initial planning for the CloudPad project. Key points:\n- Timeline: 3 months\n- Team: 4 developers, 1 designer\n- Priority features: note creation, tagging, search",
            Tags = new List<int> { 1, 2, 6 }, // Work, Meeting, Projects
            CategoryId = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"), // Business
            IsFavorite = true
        };


        // Act
        var result = async () => await _sut.AddAsync(UserId, createNoteDto);

        // Assert 
        await result.Should().ThrowAsync<InvalidCreateNoteException>();
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenCategoryWasNotFound()
    {
        // Arrange 
        var categoryId = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7");
        CreateNoteDto createNoteDto = new CreateNoteDto
        {
            Title = "Project Kickoff Notes",
            Content =
                "Initial planning for the CloudPad project. Key points:\n- Timeline: 3 months\n- Team: 4 developers, 1 designer\n- Priority features: note creation, tagging, search",
            Tags = new List<int> { 1, 2, 6 }, // Work, Meeting, Projects
            CategoryId = categoryId, // Business
            IsFavorite = true
        };

        _categoryRepository.GetByIdAsync(Arg.Is(UserId), Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = async () => await _sut.AddAsync(UserId, createNoteDto);

        // Assert 
        await result.Should().ThrowAsync<CategoryNotFoundException>("Category {0} assigned to note was not found", categoryId);
        await _categoryRepository.Received(1).GetByIdAsync(Arg.Is(UserId), Arg.Is(categoryId));
    }

    #endregion

    #region UpdateAsync

    [Theory]
    [ClassData(typeof(UpdateNoteDtoTestData))]
    public async Task UpdateAsync_ShouldUpdateNote_WhenInvoked(UpdateNoteDto updateNoteDto)
    {
        // Arrange 

        var note = new Note()
        {
            UserId = UserId,
            NoteGuid = updateNoteDto.NoteId,
            Title = "Initial Title",
            Content = "Initial content",
        };

        _noteRepository.GetByIdAsync(Arg.Is(UserId), Arg.Is(updateNoteDto.NoteId)).Returns(note);

        Category? category = null;
        if (updateNoteDto.CategoryId.HasValue)
        {
            category = new Category()
            {
                UserId = UserId,
                CategoryId = 1,
                CategoryGuid = updateNoteDto.CategoryId.Value
            };
        }

        if (updateNoteDto.CategoryId.HasValue)
        {
            _categoryRepository.GetByIdAsync(Arg.Is(UserId), 
                Arg.Is(updateNoteDto.CategoryId.Value)).Returns(category);
        }

        _noteRepository.UpdateAsync(Arg.Any<Note>()).Returns(callInfo =>
        {
            var updatedNote = callInfo.Arg<Note>();
            return updatedNote;
        });

        var tagDtos = updateNoteDto.Tags == null? new List<TagDto>(): updateNoteDto.Tags.Select(t => new TagDto()
        {
            Id = t
        }).ToList();

        _tagManagerService.UpdateNoteTagsAsync(Arg.Is(UserId), Arg.Is(updateNoteDto.NoteId),
                Arg.Any<List<int>>())
            .Returns(tagDtos);

        var expectedResult = note.ToDto();
        expectedResult.Title = updateNoteDto.Title;
        expectedResult.Content = updateNoteDto.Content;
        expectedResult.Tags = tagDtos;
        expectedResult.Category = category?.ToDto();
        expectedResult.IsFavorite = updateNoteDto.IsFavorite;
        expectedResult.IsPinned = updateNoteDto.IsPinned;
        expectedResult.IsArchived = updateNoteDto.IsArchived;
        expectedResult.Resources = [];

        // Act
        var result = await _sut.UpdateAsync(UserId, updateNoteDto);

        // Assert 
        await _noteRepository.Received(1).GetByIdAsync(Arg.Is(UserId), 
            Arg.Is(updateNoteDto.NoteId));
        await _tagManagerService.Received(1).UpdateNoteTagsAsync(Arg.Is(UserId), 
            Arg.Is(updateNoteDto.NoteId),
            Arg.Any<List<int>>());
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenNoteIdIsEmpty()
    {
        // Arrange 
        var updateNoteDto = new UpdateNoteDto
        {
            NoteId = Guid.Empty,
            Title = "Updated Title",
            Content = "Updated content",
            Tags = new List<int> { 1, 2, 3 },
            CategoryId = Guid.NewGuid(),
            IsFavorite = true
        };

        // Act
        var result = async () => await _sut.UpdateAsync(UserId, updateNoteDto);

        // Assert 
        await result.Should().ThrowAsync<InvalidNoteIdException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenNoteWasNotFound()
    {
        // Arrange 
        var updateNoteDto = new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Updated Title",
            Content = "Updated content",
            Tags = new List<int> { 1, 2, 3 },
            CategoryId = Guid.NewGuid(),
            IsFavorite = true
        };
        _noteRepository.GetByIdAsync(Arg.Is(UserId), Arg.Is(updateNoteDto.NoteId)).ReturnsNull();

        // Act
        var result = async () => await _sut.UpdateAsync(UserId, updateNoteDto);

        // Assert 
        await result.Should().ThrowAsync<NoteNotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNote_WhenCategoryWasNotFound()
    {
        // Arrange 
        var updateNoteDto = new UpdateNoteDto
        {
            NoteId = Guid.NewGuid(),
            Title = "Updated Title",
            Content = "Updated content",
            Tags = new List<int> { 1, 2, 3 },
            CategoryId = Guid.NewGuid(),
            IsFavorite = true
        };

        var note = new Note()
        {
            UserId = UserId,
            NoteGuid = updateNoteDto.NoteId,
            Title = "Initial Title",
            Content = "Initial content",
        };

        _noteRepository.GetByIdAsync(Arg.Is(UserId), Arg.Is(updateNoteDto.NoteId)).Returns(note);
        
        _categoryRepository.GetByIdAsync(Arg.Is(UserId), Arg.Is(updateNoteDto.CategoryId.Value)).ReturnsNull();

        // Act
        var result = async () => await _sut.UpdateAsync(UserId, updateNoteDto);

        // Assert 
       await result.Should()
            .ThrowAsync<CategoryNotFoundException>(
                $"Category {updateNoteDto.CategoryId} assigned to note {updateNoteDto.NoteId} was not found");
    }
    #endregion UpdateAsync

    #region RestoreAsync
    [Fact]
    public async Task RestoreAsync_ShouldReturnNoteOrNull_WhenInvoked()
    {
        // Arrange 
        var noteId = Guid.NewGuid();
        Note note = new Note()
        {
            NoteId = 1,
            UserId = UserId,
            NoteGuid = noteId,
            Title = "note title", 
            Content = "note content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt =DateTime.UtcNow,
            Tags = [],
            Category = null,
            CategoryId = null,
            Resources = [],
        };
        
         _noteRepository.RestoreAsync(UserId, noteId).Returns(Task.FromResult(note));
        
        
        var expectedResult = note.ToDto();

        // Act 
        var result =  await _sut.RestoreAsync(UserId, noteId);
        
        // Assert
        
        result.Should().BeEquivalentTo(expectedResult);
    }

    
    [Fact]
    public async Task RestoreAsync_ShouldThrowException_WhenInvokedWithInvalidNoteId()
    {
        // Arrange 
        var noteId = Guid.NewGuid(); 
        _noteRepository.GetByIdAsync(UserId, noteId).ReturnsNull();

        // Act 
        var result = async () => await _sut.RestoreAsync(UserId, noteId);
        // Assert
         await result.Should().ThrowAsync<InvalidNoteException>();
    }
    #endregion RestoreAsync
    
    #region DeleteAsync
    [Fact]
    public async Task DeleteAsync_ShouldDeleteNote_WhenInvoked()
    {
        // Arrange 
        var note = new Note()
        {
            NoteId = 1,
            UserId = UserId,
            NoteGuid = Guid.NewGuid(),
            Title = "note title", 
            Content = "note content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt =DateTime.UtcNow,
            Tags = [],
            Category = null,
            CategoryId = null,
            Resources = [],
            IsDeleted = false
        };

        Note? capturedNote = null;
        _noteRepository.GetByIdAsync(UserId, note.NoteGuid).Returns(note);
        _noteRepository.UpdateAsync(Arg.Do<Note>(note => capturedNote = note))
            .Returns(callIn =>
            {
                var deletedNote = callIn.Arg<Note>();
                return deletedNote;
            });
        
        // Act 
        await _sut.DeleteAsync(UserId, note.NoteGuid);
        
        // Assert 
        capturedNote.Should().NotBeNull();
        capturedNote.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenNoteIdIsInvalidNote()
    {
        // Arrange 
        var noteId = Guid.Empty; 
        
        // Act 
        var result = async () => await _sut.DeleteAsync(UserId, noteId);
        
        // Assert 
        await result.Should().ThrowAsync<InvalidNoteIdException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenNoteWasNotFound()
    {
        // Arrange 
        var noteId = Guid.NewGuid();
        _noteRepository.GetByIdAsync(UserId, noteId).ReturnsNull();
        
        // Act 
        var result = async () => await _sut.DeleteAsync(UserId, noteId);
        
        // Assert 
        await result.Should().ThrowAsync<NoteNotFoundException>();
    }
    #endregion DeleteAsync
    
}