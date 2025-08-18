using Gustov.Domain.Entities;

namespace Gustov.Domain.Interfaces.Repositories
{
    public interface IProductRepostory
    {
        Task<Product> Create(Product product);
        Task<List<Product>> FindAll();
        Task<Product> Update(Product product);
        Task SaveChanges();
    }
}
