using Xunit;
using FluentAssertions;
using NSubstitute;
using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using CloudPad.Core.Mappers;
using CloudPad.Core.Entities;

public class CategoryManagerServiceTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IMemoryCache _cache = Substitute.For<IMemoryCache>();
    private readonly IUserValidatorService _userValidatorService = Substitute.For<IUserValidatorService>();
    private readonly ICategoryValidatorService _categoryValidatorService = Substitute.For<ICategoryValidatorService>();
    private readonly ICategoryManagerService _sut;

    private readonly int UserId = 123;

    public CategoryManagerServiceTests()
    {
        _sut = new CategoryManagerService(_categoryRepository, _cache, _userValidatorService, _categoryValidatorService);
    }

    private const string CacheKeyPrefix = "Categories/";

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCategoryDtoIsNull()
    {
        Func<Task> act = async () => await _sut.CreateAsync(UserId, null);

        await act.Should().ThrowAsync<CategoryArgumentNullException>()
            .WithMessage("category was null");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateAsync_ShouldThrow_WhenCategoryNameIsEmpty(string invalidName)
    {
        var dto = new CreateCategoryDto { Name = invalidName };

        Func<Task> act = async () => await _sut.CreateAsync(UserId, dto);

        await act.Should().ThrowAsync<InvalidCategoryException>()
            .WithMessage("Category name cannot be null or empty");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenDuplicateCategoryNameExists()
    {
        var dto = new CreateCategoryDto { Name = "ExistingName" };
        
        _categoryValidatorService.ExistsAsync(UserId, dto.Name).Returns(true);

        Func<Task> act = async () => await _sut.CreateAsync(UserId, dto);

        await act.Should().ThrowAsync<DuplicateCategoryNameException>()
            .WithMessage("Category name already exists");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenDescriptionTooLong()
    {
        var dto = new CreateCategoryDto 
        { 
            Name = "NewCategory", 
            Description = new string('x',501) 
        };
        
        Func<Task> act = async () => await _sut.CreateAsync(UserId, dto);

        await act.Should().ThrowAsync<InvalidCategoryException>()
            .WithMessage("Category description cannot be more than 500 characters");
    }

    [Fact(Skip = "Need to fix this test")] 
    public async Task CreateAsync_ShouldCreateCategory_WhenInputsAreValid()
    {
        var dto = new CreateCategoryDto 
        { 
            Name = "ValidCategory", 
            Description = "Valid Description" 
        };

        _categoryValidatorService.ExistsAsync(UserId, dto.Name).Returns(false);
        
        var categoryEntity = dto.ToEntity();
        categoryEntity.UserId = UserId;

        _categoryRepository.CreateAsync(Arg.Is(UserId),
            Arg.Any<Category>()).Returns(callerInfo =>
        {
            var category = callerInfo.Arg<Category>();
            return category;
        });


            
        //     categoryEntity.ToDto();
        // returnedCategory.Id = Guid.NewGuid();

        var result = await _sut.CreateAsync(UserId, dto);

        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
        _cache.Received(1).Remove($"{CacheKeyPrefix}{UserId}");
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenDtoIsNull()
    {
        Func<Task> act = async () => await _sut.UpdateAsync(UserId, null);

        await act.Should().ThrowAsync<CategoryArgumentNullException>()
            .WithMessage("category was null");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenCategoryIdEmpty()
    {
        var dto = new UpdateCategoryDto 
        { 
            CategoryId = Guid.Empty, 
            Name = "Valid Name" 
        };

        Func<Task> act = async () => await _sut.UpdateAsync(UserId, dto);

        await act.Should().ThrowAsync<InvalidCategoryIdException>()
            .WithMessage("Category id cannot be empty");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task UpdateAsync_ShouldThrow_WhenCategoryNameInvalid(string invalidName)
    {
        var dto = new UpdateCategoryDto 
        { 
            CategoryId = Guid.NewGuid(), 
            Name = invalidName 
        };

        Func<Task> act = async () => await _sut.UpdateAsync(UserId, dto);

        await act.Should().ThrowAsync<InvalidCategoryException>()
            .WithMessage("Category name cannot be null or empty");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenCategoryNotFound()
    {
        var dto = new UpdateCategoryDto 
        { 
            CategoryId = Guid.NewGuid(), 
            Name = "Updated Name" 
        };

        _categoryRepository.GetByIdAsync(UserId, dto.CategoryId).Returns((Category)null);

        Func<Task> act = async () => await _sut.UpdateAsync(UserId, dto);

        await act.Should().ThrowAsync<CategoryNotFoundException>()
            .WithMessage("Category was not found");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory_WhenInputsAreValid()
    {
        var categoryGuid = Guid.NewGuid();
        var existingCategory = new Category 
        { 
            CategoryGuid = categoryGuid, 
            Name = "Old Name",
            Description=null,
            IsFavorite=false
        };
        
        var dto = new UpdateCategoryDto 
        { 
            CategoryId = categoryGuid, 
            Name = "Updated Name",
            Description="Updated Description",
            IsFavorite=true
        };

        _categoryRepository.GetByIdAsync(UserId, dto.CategoryId).Returns(existingCategory);
        _categoryValidatorService.ExistsAsync(UserId, dto.Name).Returns(false);
        _categoryRepository.UpdateAsync(UserId, Arg.Any<Category>()).Returns(args => args.Arg<Category>());

        var result = await _sut.UpdateAsync(UserId, dto);

        result.Name.Should().Be(dto.Name);
        result.Description.Should().Be(dto.Description);
        result.IsFavorite.Should().BeTrue();
        _cache.Received(1).Remove($"{CacheKeyPrefix}{UserId}");
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenGuidEmpty()
    {
        Func<Task> act = async () => await _sut.DeleteAsync(UserId, Guid.Empty);

        await act.Should().ThrowAsync<InvalidCategoryIdException>()
            .WithMessage("Category id cannot be empty");
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenCategoryNotFound()
    {
        var categoryId = Guid.NewGuid();

        _categoryRepository.GetByIdAsync(UserId, categoryId).Returns((Category)null);

        Func<Task> act = async () => await _sut.DeleteAsync(UserId, categoryId);

        await act.Should().ThrowAsync<CategoryNotFoundException>()
            .WithMessage($"Category with id {categoryId} was not found");
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkCategoryAsDeleted_WhenValidCategoryIdProvided()
    {
        var categoryId = Guid.NewGuid();
        var category = new Category { CategoryGuid = categoryId, IsDeleted = false };

        _categoryRepository.GetByIdAsync(UserId, categoryId).Returns(category);

        await _sut.DeleteAsync(UserId, categoryId);

        category.IsDeleted.Should().BeTrue();
        await _categoryRepository.Received(1).UpdateAsync(UserId, category);
        _cache.Received(1).Remove($"{CacheKeyPrefix}{UserId}");
    }

    #endregion
}
