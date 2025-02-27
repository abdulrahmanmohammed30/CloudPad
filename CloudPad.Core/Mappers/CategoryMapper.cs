﻿using CloudPad.Core.Dtos;
using CloudPad.Core.Entities;

namespace CloudPad.Core.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto()
        {
            Id = category.CategoryGuid,
            Name = category.Name,
            Description = category.Description,
            IsFavorite = category.IsFavorite
        };
    }

    public static Category ToEntity(this CategoryDto categoryDto)
    {
        return new Category()
        {
            IsFavorite = categoryDto.IsFavorite,
            Description = categoryDto.Description,
            Name = categoryDto.Name,
            CategoryGuid = categoryDto.Id,

        };
    }
    
    public static Category ToEntity(this CreateCategoryDto categoryDto)
    {
        return new Category()
        {
            IsFavorite = categoryDto.IsFavorite,
            Description = categoryDto.Description,
            Name = categoryDto.Name,
        };
    }
    
    public static Category ToEntity(this UpdateCategoryDto categoryDto)
    {
        return new Category()
        {
            IsFavorite = categoryDto.IsFavorite,
            Description = categoryDto.Description,
            Name = categoryDto.Name,
            CategoryGuid = categoryDto.CategoryId,

        };
    }

    public static List<CategoryDto> ToDtoList(this List<Category> categories)
    {
        return categories.Select(c => c.ToDto()).ToList();
    }
}