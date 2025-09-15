using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }

    public Quantity Quantity { get; private set; } = null!;
    public UnitPrice UnitPrice { get; private set; } = null!;

    private OrderItem()
    {
        // EF Core only
    }

    private OrderItem(Guid id, Guid productId, Quantity quantity, UnitPrice unitPrice)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static OrderItem Create(Guid id, Guid productId, int quantity, decimal unitPrice)
    {
        var orderQuantity = Quantity.Create(quantity);
        var orderUnitPrice = UnitPrice.Create(unitPrice);

        return new OrderItem(id, productId, orderQuantity, orderUnitPrice);
    }

    public decimal Total => Quantity.Value * UnitPrice.Value;
}
