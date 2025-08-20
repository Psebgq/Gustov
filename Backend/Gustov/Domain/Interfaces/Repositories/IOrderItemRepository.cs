using Gustov.Domain.Entities;

namespace Gustov.Domain.Interfaces.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> Create(OrderItem orderItem);
        Task<List<OrderItem>> FindAll();
        Task<OrderItem> Update(OrderItem orderItem);
        Task<OrderItem?> FindOne(int orderItemId);
        Task<List<OrderItem>> FindBySaleId(int saleId);
    }
}