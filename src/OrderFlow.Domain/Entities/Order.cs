using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> orderItems;
    public IReadOnlyList<OrderItem> OrderItems => orderItems.AsReadOnly();

    private Order(Guid id, Guid customerId, DateTime orderDate, OrderStatus status, IEnumerable<OrderItem> orderItems)
    {
        Id = id;
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = status;
        this.orderItems = [.. orderItems ?? []];
    }

    public static Order Create(Guid id, Guid customerId, DateTime orderDate, IEnumerable<OrderItem> orderItems)
    {
        if (orderItems == null || !orderItems.Any())
        {
            throw new DomainValidationException(
                "Order.NoItems",
                "Order must contain at least one item."
            );
        }

        return new Order(id, customerId, orderDate, OrderStatus.Pending, orderItems);
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
        var itemToRemove = orderItems.FirstOrDefault(oi => oi.Id == itemId);
        if (itemToRemove == null)
        {
            throw new DomainValidationException(
                "OrderItem.NotFound",
                $"Order item with ID '{itemId}' was not found.",
                itemId
            );
        }

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

    public decimal GetTotal() => orderItems.Sum(item => item.GetTotal());

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
