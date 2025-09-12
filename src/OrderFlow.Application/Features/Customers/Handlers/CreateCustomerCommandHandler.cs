using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Customers.Commands;
using OrderFlow.Application.Features.Customers.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    private readonly ICustomerRepository customerRepository = customerRepository;

    public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var existing = await customerRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existing is not null)
        {
            return Result<Guid>.Failure(
                "Customer.EmailAlreadyUsed",
                $"Email '{command.Email}' is already in use."
            );
        }

        var customerCreationResult = CustomerFactory.CreateCustomer(
            Guid.NewGuid(),
            command.FullName,
            command.Email
        );

        var customer = customerCreationResult.Value!;
        await customerRepository.AddAsync(customer, cancellationToken);

        return Result<Guid>.Success(customer.Id);
    }
}
