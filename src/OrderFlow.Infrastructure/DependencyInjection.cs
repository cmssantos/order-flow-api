using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Interfaces;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Interceptors;
using OrderFlow.Infrastructure.Logging;
using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Infrastructure.Repositories;
using OrderFlow.Infrastructure.Security;

namespace OrderFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
      IConfiguration configuration)
    {
        // === Context Access ===
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, HttpUserContext>();

        // === Persistence & Unit of Work ===
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // === Repositories ===
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // === Logging ===
        services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLogger<>));

        // === Database Configuration ===
        services.AddDbContext<OrderFlowDbContext>(options =>
        {
            options.UseInMemoryDatabase("OrderFlowDb");
        });

        return services;
    }
}
