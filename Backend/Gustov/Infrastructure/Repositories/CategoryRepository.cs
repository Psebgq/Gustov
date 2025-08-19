using Gustov.Domain.Entities;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gustov.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GustovDbContext _dbContext;

        public CategoryRepository(GustovDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> Create(Category category)
        {
            category.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            category.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            var result = await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Category>> FindAll()
        {
            return await _dbContext.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    SortOrder = c.SortOrder,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<Category> Update(Category category)
        {
            category.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
    }
}