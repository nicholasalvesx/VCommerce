using VCommerce.Modules.Core.Application.Interfaces;
using VCommerce.Modules.Core.Application.Services;

namespace VCommerce.Modules.Core.Application.Configuration;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}