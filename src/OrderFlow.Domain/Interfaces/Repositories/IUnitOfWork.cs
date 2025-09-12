namespace OrderFlow.Domain.Interfaces.Repositories;

public interface IUnitOfWork: IDisposable
{
    ICustomerRepository Customers { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
