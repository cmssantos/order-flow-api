using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Orders.Services;

public class OrderFactory
{
    public static Result<Order> CreateOrderWithItems(
        Guid customerId,
        IEnumerable<OrderItemInput> itemInputs,
        IEnumerable<Product> products)
    {
        var order = Order.Create(customerId, []);

        foreach (var itemInput in itemInputs)
        {
            var product = products.FirstOrDefault(p => p.Id == itemInput.ProductId);
            if (product is null)
            {
                return Result<Order>.Failure(
                    "Order.ProductNotFound",
                    $"Product with ID '{itemInput.ProductId}' was not found."
                );
            }

            var orderItem = OrderItem.Create(
                Guid.NewGuid(),
                product.Id,
                itemInput.Quantity,
                product.Price.Value
            );

            order.AddItem(orderItem);
        }

        return Result<Order>.Success(order);
    }
}
