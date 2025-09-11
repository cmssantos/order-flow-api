using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Customers.Commands;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<DeleteCustomerCommand, Result<Unit>>
{
    private readonly ICustomerRepository customerRepository = customerRepository;

    public async Task<Result<Unit>> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.Id);
        if (customer is null)
        {
            return Result<Unit>.Failure(
               "Customer.NotFound",
               $"Customer with ID {command.Id} not found."
            );
        }

        await customerRepository.DeleteAsync(command.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
