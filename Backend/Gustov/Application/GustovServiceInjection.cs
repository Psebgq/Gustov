using Gustov.Application.Services;

namespace Gustov.Application
{
    public static class GustovServiceInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection collection)
        {
            collection.AddTransient<CategoryService>();
            collection.AddTransient<ProductService>();
            collection.AddTransient<OrderItemService>();
            collection.AddTransient<SaleService>();
            return collection;
        }
    }
}
