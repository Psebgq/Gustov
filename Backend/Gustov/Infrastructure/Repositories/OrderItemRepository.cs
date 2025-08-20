using Gustov.Domain.Entities;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gustov.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly GustovDbContext _dbContext;

        public OrderItemRepository(GustovDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItem> Create(OrderItem orderItem)
        {
            orderItem.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            orderItem.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            var result = await _dbContext.OrderItems.AddAsync(orderItem);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task CreateRange(List<OrderItem> orderItems)
        {
            await _dbContext.OrderItems.AddRangeAsync(orderItems);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderItem>> FindAll()
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Sale)
                .Include(oi => oi.Category)
                .Where(oi => oi.IsActive)
                .OrderByDescending(oi => oi.CreatedAt)
                .Select(oi => new OrderItem
                {
                    Id = oi.Id,
                    SaleId = oi.SaleId,
                    CategoryId = oi.CategoryId,
                    Name = oi.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice,
                    IsActive = oi.IsActive,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt,
                    Sale = oi.Sale,
                    Category = oi.Category
                })
                .ToListAsync();
        }

        public async Task<List<OrderItem>> FindBySale(int saleId)
        {
            return await _dbContext.OrderItems
                .Where(oi => oi.SaleId == saleId)
                .Select(oi => new OrderItem
                {
                    Id = oi.Id,
                    SaleId = oi.SaleId,
                    CategoryId = oi.CategoryId,
                    Name = oi.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice,
                    IsActive = oi.IsActive,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt,
                    Sale = oi.Sale,
                    Category = oi.Category
                })
                .ToListAsync();
        }

        public async Task<OrderItem?> FindOne(int orderItemId)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Sale)
                .Include(oi => oi.Category)
                .Where(oi => oi.Id == orderItemId && oi.IsActive)
                .Select(oi => new OrderItem
                {
                    Id = oi.Id,
                    SaleId = oi.SaleId,
                    CategoryId = oi.CategoryId,
                    Name = oi.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice,
                    IsActive = oi.IsActive,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt,
                    Sale = oi.Sale,
                    Category = oi.Category
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderItem>> FindBySaleId(int saleId)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Category)
                .Where(oi => oi.SaleId == saleId && oi.IsActive)
                .OrderBy(oi => oi.Name)
                .Select(oi => new OrderItem
                {
                    Id = oi.Id,
                    SaleId = oi.SaleId,
                    CategoryId = oi.CategoryId,
                    Name = oi.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice,
                    IsActive = oi.IsActive,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt,
                    Category = oi.Category
                })
                .ToListAsync();
        }
    }
}