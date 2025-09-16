using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class OrderRepository(OrderFlowDbContext context)
    : Repository<Order>(context), IOrderRepository
{
    private readonly DbSet<Order> dbSet = context.Set<Order>();

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
        =>  await dbSet
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId)
                .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
        =>  await dbSet
                .AsNoTracking()
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToListAsync(cancellationToken);
}
