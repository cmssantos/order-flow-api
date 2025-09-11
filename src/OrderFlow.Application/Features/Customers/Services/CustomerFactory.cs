using OrderFlow.Application.Common.Models;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Customers.Services;

public class CustomerFactory
{
    public static Result<Customer> CreateCustomer(Guid customerId, string name, string email)
    {
        var customer = Customer.Create(customerId, name, email);
        return Result<Customer>.Success(customer);
    }
}
