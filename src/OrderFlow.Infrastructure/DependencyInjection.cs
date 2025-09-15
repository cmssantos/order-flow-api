using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Interceptors;
using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Infrastructure.Repositories;
using OrderFlow.Infrastructure.Security;

namespace OrderFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
      IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IUserContext, HttpUserContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddDbContext<OrderFlowDbContext>(options =>
            options.UseInMemoryDatabase("OrderFlowDb"));

        return services;
    }
}
