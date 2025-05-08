using VCommerce.Modules.Core.Application.Configuration;

namespace VCommerce.Api.Configuration;

public static class DependencyInjectionContainer
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddApplicationServices();
    }
}