using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Customers.Commands;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<UpdateCustomerCommand, Result<Unit>>
{
    private readonly ICustomerRepository customerRepository = customerRepository;

    public async Task<Result<Unit>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.Id);
        if (customer is null)
        {
            return Result<Unit>.Failure(
                "Customer.NotFound",
                $"Customer with ID {command.Id} not found."
            );

        }

        var existing = await customerRepository.GetByEmailAsync(command.Email);
        if (existing is not null && existing.Id != command.Id)
        {
            return Result<Unit>.Failure(
                "Customer.EmailAlreadyUsed",
                $"Email '{command.Email}' is already in use."
            );
        }

        customer.Update(command.FullName, command.Email);
        await customerRepository.UpdateAsync(customer);

        return Result<Unit>.Success(Unit.Value);
    }
}
