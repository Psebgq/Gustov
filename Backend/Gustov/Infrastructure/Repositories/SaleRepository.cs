using Gustov.Domain.Entities;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gustov.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly GustovDbContext _dbContext;

        public SaleRepository(GustovDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sale> Create(Sale sale)
        {
            sale.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            var result = await _dbContext.Sales.AddAsync(sale);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Sale>> FindAll()
        {
            return await _dbContext.Sales
                .Include(s => s.OrderItems)
                    .ThenInclude(oi => oi.Category)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new Sale
                {
                    Id = s.Id,
                    SubTotal = s.SubTotal,
                    TipAmount = s.TipAmount,
                    Total = s.Total,
                    CashRecieved = s.CashRecieved,
                    CashChange = s.CashChange,
                    CreatedAt = s.CreatedAt,
                    OrderItems = s.OrderItems.Where(oi => oi.IsActive).ToList()
                })
                .ToListAsync();
        }

        public async Task<Sale?> FindOne(int saleId)
        {
            return await _dbContext.Sales
                .Include(s => s.OrderItems)
                    .ThenInclude(oi => oi.Category)
                .Where(s => s.Id == saleId)
                .Select(s => new Sale
                {
                    Id = s.Id,
                    SubTotal = s.SubTotal,
                    TipAmount = s.TipAmount,
                    Total = s.Total,
                    CashRecieved = s.CashRecieved,
                    CashChange = s.CashChange,
                    CreatedAt = s.CreatedAt,
                    OrderItems = s.OrderItems.Where(oi => oi.IsActive).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Sale> Update(Sale sale)
        {
            _dbContext.Sales.Update(sale);
            await _dbContext.SaveChangesAsync();
            return sale;
        }
    }
}