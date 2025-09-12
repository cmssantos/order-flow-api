using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Customers.Commands;
using OrderFlow.Application.Features.Customers.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class CreateCustomerCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Customers.GetByEmailAsync(command.Email, cancellationToken);
        if (existing is not null)
        {
            return Result<Guid>.Failure(
                "Customer.EmailAlreadyUsed",
                $"Email '{command.Email}' is already in use."
            );
        }

        var customerCreationResult = CustomerFactory.CreateCustomer(
            command.FullName,
            command.Email
        );

        var customer = customerCreationResult.Value!;
        await unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(customer.Id);
    }
}
