using Gustov.Domain.Entities;

namespace Gustov.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Create(Category category);
        Task<List<Category>> FindAll();
        Task<Category> Update(Category category);
    }
}