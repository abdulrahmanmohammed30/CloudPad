using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace NoteTakingApp.Tests.Unit.Services;

public class CategoryValidatorServiceTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMemoryCache _cache = Substitute.For<IMemoryCache>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();
    private readonly ICategoryValidatorService _sut;

    private readonly int _userId = 10;

    public CategoryValidatorServiceTests()
    {
        _sut = new CategoryValidatorService(_categoryRepository, _userValidatorService);
    }

    #region ExistsAsync by Name Tests

    [Fact]
    public async Task ExistsAsync_ByName_ShouldReturnTrue_WhenCategoryExists()
    {
        // Arrange
        var categoryName = "TestCategory";
        _categoryRepository.ExistsAsync(_userId, categoryName: categoryName).Returns(true);

        // Act
        var exists = await _sut.ExistsAsync(_userId, categoryName);

        // Assert
        exists.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ExistsAsync_ShouldThrow_WhenInvalidCategoryName(string invalidName)
    {
        // Act
        Func<Task> act = async () => await _sut.ExistsAsync(_userId, invalidName);

        // Assert
        await act.Should().ThrowAsync<InvalidCategoryException>()
            .WithMessage($"{invalidName} is not a valid category name");
    }

    #endregion

    #region ExistsAsync by Guid Tests

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenCategoryExistsByGuid()
    {
        // Arrange
        var categoryGuid = Guid.NewGuid();
        _categoryRepository.ExistsAsync(_userId, categoryGuid).Returns(true);

        // Act
        var exists = await _sut.ExistsAsync(_userId, categoryGuid);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldThrow_WhenGuidEmpty()
    {
        // Act
        Func<Task> act = () => _sut.ExistsAsync(_userId, Guid.Empty);

        // Assert
        await act.Should().ThrowAsync<InvalidCategoryException>()
            .WithMessage("Category id cannot be empty");
    }

    #endregion
}