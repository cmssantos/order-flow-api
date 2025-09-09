using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces.Repositories;

public interface IProductRepository: IRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku);
    Task<IReadOnlyCollection<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
}
