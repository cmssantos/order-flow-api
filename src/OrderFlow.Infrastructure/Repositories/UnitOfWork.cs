using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class UnitOfWork(
    OrderFlowDbContext dbContext,
    ICustomerRepository customerRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository): IUnitOfWork
{
    private readonly OrderFlowDbContext dbContext = dbContext;

    public ICustomerRepository Customers { get; } = customerRepository;
    public IProductRepository Products { get; } = productRepository;
    public IOrderRepository Orders { get; } = orderRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);

    public void Dispose() => dbContext.Dispose();
}
