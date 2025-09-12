using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Customers.Commands;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class DeleteCustomerCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<DeleteCustomerCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetByIdAsync(command.Id, cancellationToken);
        if (customer is null)
        {
            return Result<Unit>.Failure(
               "Customer.NotFound",
               $"Customer with ID {command.Id} not found."
            );
        }

        await unitOfWork.Customers.DeleteAsync(command.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
