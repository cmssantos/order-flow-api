using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Customers.Commands;

public record DeleteCustomerCommand(Guid Id): IRequest<Result<Unit>>;
