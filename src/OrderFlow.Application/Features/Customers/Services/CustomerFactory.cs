using OrderFlow.Application.Common.Models;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Customers.Services;

public class CustomerFactory
{
    public static Result<Customer> CreateCustomer(string name, string email)
    {
        var customer = Customer.Create(name, email);
        return Result<Customer>.Success(customer);
    }
}
