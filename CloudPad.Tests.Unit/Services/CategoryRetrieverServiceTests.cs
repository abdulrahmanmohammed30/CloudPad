using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.Services;
using CloudPad.Core.ServiceContracts;

public class CategoryRetrieverServiceTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMemoryCache _cache = Substitute.For<IMemoryCache>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();
    private readonly CategoryRetrieverService _sut;

    private readonly int _userId = 10;

    public CategoryRetrieverServiceTests()
    {
        _sut = new CategoryRetrieverService(_categoryRepository, _cache, _userValidatorService);
    }

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryEntity = new Category { CategoryGuid = categoryId, Name = "TestCategory" };
        _categoryRepository.GetByIdAsync(_userId, categoryId).Returns(categoryEntity);

        // Act
        var result = await _sut.GetByIdAsync(_userId, categoryId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("TestCategory");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenCategoryIdEmpty()
    {
        // Arrange
        var invalidGuid = Guid.Empty;

        // Act
        Func<Task> act = async () => await _sut.GetByIdAsync(_userId, invalidGuid);

        // Assert
        await act.Should().ThrowAsync<InvalidCategoryIdException>()
            .WithMessage("Category id cannot be empty");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryNotFound()
    {
        // Arrange
        var missingGuid = Guid.NewGuid();
        _categoryRepository.GetByIdAsync(_userId, missingGuid).Returns((Category)null!);

        // Act
        var result = await _sut.GetByIdAsync(_userId, missingGuid);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetByNameAsync Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByNameAsync_ShouldThrow_WhenInvalidCategoryNameProvided(string invalidName)
    {
        // Act
        Func<Task> act = async () => await _sut.GetByNameAsync(_userId, invalidName);

        // Assert
        await act.Should().ThrowAsync<InvalidCategoryException>()
            .WithMessage($"{invalidName} is not a valid category name");
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCategory_WhenExists()
    {
        // Arrange
        string categoryName = "Development";
        var existingCategory = new Category { Name = categoryName, CategoryGuid = Guid.NewGuid() };
        _categoryRepository.GetByNameAsync(_userId, categoryName).Returns(existingCategory);

        // Act
        var result = await _sut.GetByNameAsync(_userId, categoryName);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(categoryName);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        string missingName = "NonExistent";
        _categoryRepository.GetByNameAsync(_userId, missingName).Returns((Category)null!);

        // Act
        var result = await _sut.GetByNameAsync(_userId, missingName);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ShouldReturnCategories_FromCacheIfExists()
    {
        // Arrange
        var cachedCategories = new List<CategoryDto> { new CategoryDto { Name = "CachedCategory" } };
        object cacheEntry = cachedCategories;
        _cache.TryGetValue($"Categories/{_userId}", out cacheEntry).Returns(true);

        // Act
        var result = await _sut.GetAllAsync(_userId);

        // Assert
        result.Should().BeEquivalentTo(cachedCategories);
        await _categoryRepository.DidNotReceive().GetAllAsync(_userId);
    }

    [Fact]
    public async Task GetAllAsync_ShouldFetchCategories_FromRepositoryAndSetCache_WhenCacheEmpty()
    {
        // Arrange
        object? cacheEntry = null;
        var categoryEntities = new List<Category>
        {
            new Category { Name = "RepoCategory1" },
            new Category { Name = "RepoCategory2" }
        };

        var memoryCacheEntry = Substitute.For<ICacheEntry>();
        _cache.CreateEntry(Arg.Any<object>()).Returns(memoryCacheEntry);

        _cache.TryGetValue($"Categories/{_userId}", out cacheEntry).Returns(false);
        _categoryRepository.GetAllAsync(_userId).Returns(categoryEntities);

        // Act
        var result = await _sut.GetAllAsync(_userId);

        // Assert
        result.Should().HaveCount(2)
            .And.Contain(x => x.Name == "RepoCategory1")
            .And.Contain(x => x.Name == "RepoCategory2");
    }

    #endregion

    #region FindCategoryIdByGuidAsync Tests

    [Fact]
    public async Task FindCategoryIdByGuidAsync_ShouldReturnCategoryId_WhenCategoryFound()
    {
        // Arrange
        var categoryGuid = Guid.NewGuid();
        _categoryRepository.FindCategoryIdByGuidAsync(_userId, categoryGuid).Returns(42);

        // Act
        var result = await _sut.FindCategoryIdByGuidAsync(_userId, categoryGuid);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task FindCategoryIdByGuidAsync_ShouldReturnNull_WhenCategoryNotFound()
    {
        // Arrange
        var missingGuid = Guid.NewGuid();
        _categoryRepository.FindCategoryIdByGuidAsync(_userId, missingGuid).Returns((int?)null);

        // Act
        var result = await _sut.FindCategoryIdByGuidAsync(_userId, missingGuid);

        // Assert
        result.Should().BeNull();
    }

    #endregion
}
