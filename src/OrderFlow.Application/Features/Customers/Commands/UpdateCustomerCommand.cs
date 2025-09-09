namespace OrderFlow.Application.Features.Customers.Commands;

public record UpdateCustomerCommand(Guid Id, string FullName, string Email);
