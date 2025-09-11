using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public CustomerName Name { get; private set; }
    public Email Email { get; private set; }

    private Customer(Guid id, CustomerName name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public static Customer Create(Guid id, string name, string email)
    {
        var customerName = CustomerName.Create(name);
        var customerEmail = Email.Create(email);

        return new Customer(id, customerName, customerEmail);
    }

    public void Update(string name, string email)
    {
        var updatedName = CustomerName.Create(name);
        var updatedEmail = Email.Create(email);

        Name = updatedName;
        Email = updatedEmail;
    }
}
