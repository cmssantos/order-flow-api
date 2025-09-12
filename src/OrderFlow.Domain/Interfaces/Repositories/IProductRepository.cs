using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces.Repositories;

public interface IProductRepository: IRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Product>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    );
}
