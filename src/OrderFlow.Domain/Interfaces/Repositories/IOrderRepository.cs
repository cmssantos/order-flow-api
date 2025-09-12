using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces.Repositories;

public interface IOrderRepository: IRepository<Order>
{
    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
