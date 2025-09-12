using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Customers.Commands;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class UpdateCustomerCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<UpdateCustomerCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetByIdAsync(command.Id, cancellationToken);
        if (customer is null)
        {
            return Result<Unit>.Failure(
                "Customer.NotFound",
                $"Customer with ID {command.Id} not found."
            );
        }

        var existing = await unitOfWork.Customers.GetByEmailAsync(command.Email, cancellationToken);
        if (existing is not null && existing.Id != command.Id)
        {
            return Result<Unit>.Failure(
                "Customer.EmailAlreadyUsed",
                $"Email '{command.Email}' is already in use."
            );
        }

        customer.Update(command.FullName, command.Email);
        await unitOfWork.Customers.UpdateAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
