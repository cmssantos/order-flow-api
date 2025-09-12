using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces.Repositories;

public interface ICustomerRepository: IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
