using OrderFlow.Domain.Common;
using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities;

public class Customer: BaseEntity
{
    public CustomerName Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    private Customer()
    {
        // For EF Core
    }

    private Customer(CustomerName name, Email email)
    {
        Name = name;
        Email = email;
    }

    public static Customer Create(string name, string email)
    {
        var customerName = CustomerName.Create(name);
        var customerEmail = Email.Create(email);

        return new Customer(customerName, customerEmail);
    }

    public void Update(string name, string email)
    {
        var updatedName = CustomerName.Create(name);
        var updatedEmail = Email.Create(email);

        Name = updatedName;
        Email = updatedEmail;
    }
}
