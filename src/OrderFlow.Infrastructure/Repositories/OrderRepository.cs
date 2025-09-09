using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Infrastructure.Repositories;

public class OrderRepository: Repository<Order>, IOrderRepository
{
    public Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }
}
