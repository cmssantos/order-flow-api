using MediatR;
using OrderFlow.Application.DTOs;

namespace OrderFlow.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid Id): IRequest<OrderDto?>;
