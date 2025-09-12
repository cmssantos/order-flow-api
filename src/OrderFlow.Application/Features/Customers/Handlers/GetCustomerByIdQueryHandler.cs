using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Customers.Queries;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetByIdAsync(query.Id, cancellationToken);
        if (customer == null)
        {
            return Result<CustomerDto>.Failure(
                "Customer.NotFound",
                $"Customer with ID {query.Id} not found."
            );
        }

        var customerDto = MapToDto(customer);

        return Result<CustomerDto>.Success(customerDto);
    }

    private static CustomerDto MapToDto(Domain.Entities.Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name.Value,
            Email = customer.Email.Value
        };
    }
}
