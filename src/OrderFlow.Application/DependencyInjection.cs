using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Features.Orders.Services;

namespace OrderFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<OrderFactory>();

        return services;
    }
}
