using OrderFlow.Domain.Common;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.Entities;

public class Order: BaseEntity
{
    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems => orderItems.AsReadOnly();

    private Order()
    {
        // For EF Core
    }

    private Order(Guid customerId, OrderStatus status, IEnumerable<OrderItem> orderItems)
    {
        CustomerId = customerId;
        Status = status;
        this.orderItems = [.. orderItems ?? []];
    }

    public static Order Create(Guid customerId, IEnumerable<OrderItem> orderItems)
    {
        if (orderItems == null || !orderItems.Any())
        {
            throw new DomainValidationException(
                "Order.NoItems",
                "Order must contain at least one item."
            );
        }

        return new Order(customerId, OrderStatus.Pending, orderItems);
    }

    public void AddItem(OrderItem item)
    {
        if (item == null)
        {
            throw new DomainValidationException(
                "OrderItem.Null",
                "Order item cannot be null."
            );
        }

        if (orderItems.Any(oi => oi.Id == item.Id))
        {
            throw new DomainValidationException(
                "OrderItem.Duplicate",
                "Order item with the same ID already exists.",
                item.Id
            );
        }

        orderItems.Add(item);
    }

    public void RemoveItem(Guid itemId)
    {
        var itemToRemove = orderItems.FirstOrDefault(oi => oi.Id == itemId) ??
            throw new DomainValidationException(
                "OrderItem.NotFound",
                $"Order item with ID '{itemId}' was not found.",
                itemId
            );

        orderItems.Remove(itemToRemove);
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!IsValidStatusTransition(Status, newStatus))
        {
            throw new DomainValidationException(
                "Order.StatusInvalidTransition",
                $"Cannot change order status from {Status} to {newStatus}.",
                Status,
                newStatus
            );
        }

        Status = newStatus;
    }

    public decimal Total => orderItems.Sum(item => item.Total);

    private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
    {
        return current switch
        {
            OrderStatus.Pending => next is OrderStatus.Processing or OrderStatus.Cancelled,
            OrderStatus.Processing => next is OrderStatus.Shipped or OrderStatus.Cancelled,
            OrderStatus.Shipped => next == OrderStatus.Delivered,
            OrderStatus.Delivered => false,
            OrderStatus.Cancelled => false,
            _ => false
        };
    }
}
