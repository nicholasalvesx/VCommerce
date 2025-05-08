using VCommerce.Modules.Core.Infra.Repositories;

namespace VCommerce.Modules.Core.Infra.Configuration;

public static class DependencyInjection
{
    public static void AddInfraServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}