namespace OrderFlow.Application.Features.Orders.Requests;

public record CreateOrderRequest(Guid CustomerId, List<OrderItemInputRequest> Items);
public record OrderItemInputRequest(Guid ProductId, int Quantity);
