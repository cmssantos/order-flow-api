using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Customers.Commands;

public record UpdateCustomerCommand(Guid Id, string FullName, string Email): IRequest<Result<Unit>>;
