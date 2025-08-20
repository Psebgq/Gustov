using Gustov.Domain.Entities;

namespace Gustov.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Create(Product product);
        Task<List<Product>> FindAll();
        Task<Product?> FindOne(int productId);
        Task<List<Product>> FindByCategory(int id);
        Task<Product> Update(Product product);
    }
}