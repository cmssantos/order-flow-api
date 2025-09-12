using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class CustomerRepository(OrderFlowDbContext context): Repository<Customer>(context), ICustomerRepository
{
    private readonly OrderFlowDbContext context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<Customer> dbSet = context.Set<Customer>();

    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
