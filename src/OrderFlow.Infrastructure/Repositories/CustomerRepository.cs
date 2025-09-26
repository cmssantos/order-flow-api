using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class CustomerRepository(OrderFlowDbContext context)
    : Repository<Customer>(context), ICustomerRepository
{
    private readonly DbSet<Customer> dbSet = context.Set<Customer>();

    public async Task<Customer?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
        => await dbSet.FirstOrDefaultAsync(p => p.Email.Value == email, cancellationToken);
}
