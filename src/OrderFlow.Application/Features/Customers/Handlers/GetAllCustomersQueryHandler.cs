using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Customers.Queries;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Customers.Handlers;

public class GetAllCustomersQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<List<CustomerDto>>> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken)
    {
        var customers = await unitOfWork.Customers.GetAllAsync(cancellationToken);

        var customersDto = new List<CustomerDto>();
        foreach (var customer in customers)
        {
            customersDto.Add(new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value
            });
        }

        return Result<List<CustomerDto>>.Success(customersDto);
    }
}
