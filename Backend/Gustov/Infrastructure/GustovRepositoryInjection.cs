using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.Database;
using Gustov.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gustov.Infrastructure
{
    public static class GustovRepositoryInjection
    {
        public static IServiceCollection RegisterDb(this IServiceCollection collection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLConnection");
            collection.AddDbContext<GustovDbContext>(options =>
            options.UseSqlServer(connectionString));
            return collection;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection collection)
        {
            collection.AddTransient<IProductRepository, ProductRepository>();
            collection.AddTransient<ICategoryRepository, CategoryRepository>();
            collection.AddTransient<IOrderItemRepository, OrderItemRepository>();
            collection.AddTransient<ISaleRepository, SaleRepository>();
            return collection;
        }
    }
}
