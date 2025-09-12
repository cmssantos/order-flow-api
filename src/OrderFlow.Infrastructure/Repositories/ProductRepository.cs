using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class ProductRepository(OrderFlowDbContext context): Repository<Product>(context), IProductRepository
{
    private readonly OrderFlowDbContext context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<Product> dbSet = context.Set<Product>();

    public Task<IReadOnlyCollection<Product>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
