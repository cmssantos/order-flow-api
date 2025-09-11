using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Orders.Queries;

public record GetAllOrdersQuery: IRequest<Result<List<OrderDto>>>;
