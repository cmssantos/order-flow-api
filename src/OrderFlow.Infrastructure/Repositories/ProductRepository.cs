using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Infrastructure.Repositories;

public class ProductRepository: Repository<Product>, IProductRepository
{
    public Task<IReadOnlyCollection<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetBySkuAsync(string sku)
    {
        throw new NotImplementedException();
    }
}
