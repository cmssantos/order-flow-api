using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Customers.Commands;

public record CreateCustomerCommand(string FullName, string Email) : IRequest<Result<Guid>>;
