using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class ProductRepository(OrderFlowDbContext context)
    : Repository<Product>(context), IProductRepository
{
    private readonly DbSet<Product> dbSet = context.Set<Product>();

    public async Task<IReadOnlyCollection<Product>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
        =>  await dbSet.Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken);

    public async Task<Product?> GetBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default)
        =>  await dbSet.FirstOrDefaultAsync(p => p.Sku.Value == sku, cancellationToken);
}
