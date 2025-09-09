using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Application.Features.Orders.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Orders.Handlers;

public class CreateOrderCommandHandler(
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    OrderFactory orderFactory): IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IProductRepository productRepository = productRepository;
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly OrderFactory orderFactory = orderFactory;

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var productIds = request.Items
            .Select(i => i.ProductId)
            .ToList();
        var products = await productRepository.GetByIdsAsync(productIds);

        var orderCreationResult = OrderFactory
            .CreateOrderWithItems(Guid.NewGuid(), request.CustomerId, request.Items, products);

        if (!orderCreationResult.IsSuccess)
        {
            return Result<Guid>
                .Failure(orderCreationResult.Error ?? "An unexpected error occurred.");
        }

        var order = orderCreationResult.Value!;
        await orderRepository.AddAsync(order);

        return Result<Guid>.Success(order.Id);
    }
}
