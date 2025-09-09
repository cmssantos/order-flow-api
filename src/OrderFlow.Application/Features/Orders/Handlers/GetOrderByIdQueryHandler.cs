using MediatR;
using OrderFlow.Application.DTOs;
using OrderFlow.Application.Features.Orders.Queries;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Orders.Handlers;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IProductRepository productRepository = productRepository;

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id);
        if (order == null)
        {
            return null;
        }

        var orderDto = new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            TotalAmount = order.GetTotal(),
            Items = []
        };

        foreach (var item in order.OrderItems)
        {
            // Para enriquecer o DTO, buscamos o nome do produto.
            // Em um cenário real, isso poderia ser otimizado com uma única consulta.
            var product = await productRepository.GetByIdAsync(item.ProductId);

            orderDto.Items.Add(new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = product?.Name.Value ?? "Produto não encontrado",
                Quantity = item.Quantity.Value,
                UnitPrice = item.UnitPrice.Value,
                Total = item.GetTotal()
            });
        }

        return orderDto;
    }
}
