using Gustov.Domain.Entities;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                SortOrder = category.SortOrder,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public static List<CategoryDto> ToDto(IEnumerable<Category> categories)
        {
            return categories.Select(ToDto).ToList();
        }

        public static Category ToEntity(CreateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                SortOrder = dto.SortOrder,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        public static Category ToEntity(UpdateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                SortOrder = dto.SortOrder,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        public static void UpdateEntity(Category category, UpdateCategoryDto dto)
        {
            category.Name = dto.Name;
            category.SortOrder = dto.SortOrder;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;
        }
    }
}