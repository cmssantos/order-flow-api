using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Orders.Commands;

public record CreateOrderCommand(Guid CustomerId, IReadOnlyCollection<OrderItemInput> Items) : IRequest<Result<Guid>>;
public record OrderItemInput(Guid ProductId, int Quantity);
