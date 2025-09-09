using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Repositories;

namespace OrderFlow.Infrastructure.Repositories;

public class CustomerRepository: Repository<Customer>, ICustomerRepository
{
    public Task<Customer?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}
