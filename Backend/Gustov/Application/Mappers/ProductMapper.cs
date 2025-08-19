using Gustov.Domain.Entities;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                SortOrder = product.SortOrder,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryName = product.Category?.Name
            };
        }

        public static List<ProductDto> ToDto(IEnumerable<Product> products)
        {
            return products.Select(ToDto).ToList();
        }

        public static Product ToEntity(CreateProductDto dto)
        {
            return new Product
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                SortOrder = dto.SortOrder,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image,
                IsActive = dto.IsActive
            };
        }

        public static Product ToEntity(UpdateProductDto dto)
        {
            return new Product
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                SortOrder = dto.SortOrder,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image,
                IsActive = dto.IsActive
            };
        }

        public static void UpdateEntity(Product product, UpdateProductDto dto)
        {
            product.CategoryId = dto.CategoryId;
            product.Name = dto.Name;
            product.SortOrder = dto.SortOrder;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Image = dto.Image;
            product.IsActive = dto.IsActive;
        }
    }
}