using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id): IRequest<Result<CustomerDto>>;
