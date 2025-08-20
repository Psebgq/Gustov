using Gustov.Domain.Entities;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gustov.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GustovDbContext _dbContext;

        public ProductRepository(GustovDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> Create(Product product)
        {
            product.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            var result = await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Product>> FindAll()
        {
            return await _dbContext.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .Select(p => new Product
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    SortOrder = p.SortOrder,
                    Description = p.Description,
                    Price = p.Price,
                    Image = p.Image,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<Product?> FindOne(int productId)
        {
            return await _dbContext.Products
                .Where(p => p.Id == productId)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .Select(p => new Product
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    SortOrder = p.SortOrder,
                    Description = p.Description,
                    Price = p.Price,
                    Image = p.Image,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> FindByCategory(int categoryId)
        {
            return await _dbContext.Products
                .Where(p => p.IsActive && p.CategoryId == categoryId)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .Select(p => new Product
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Name = p.Name,
                    SortOrder = p.SortOrder,
                    Description = p.Description,
                    Price = p.Price,
                    Image = p.Image,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<Product> Update(Product product)
        {
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}