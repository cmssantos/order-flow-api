using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Orders.Commands;

public class CreateOrderCommand: IRequest<Result<Guid>>
{
    public Guid CustomerId { get; set; }
    public required List<OrderItemInput> Items { get; set; }

    public class OrderItemInput
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
