using Gustov.Domain.Entities;

namespace Gustov.Domain.Interfaces.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> Create(Sale sale);
        Task<List<Sale>> FindAll();
        Task<Sale> Update(Sale sale);
        Task<Sale?> FindOne(int saleId);
    }
}