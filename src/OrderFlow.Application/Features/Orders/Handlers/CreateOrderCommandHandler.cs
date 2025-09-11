using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Application.Features.Orders.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Orders.Handlers;

public class CreateOrderCommandHandler(IProductRepository productRepository, IOrderRepository orderRepository)
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IProductRepository productRepository = productRepository;
    private readonly IOrderRepository orderRepository = orderRepository;

    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var productIds = command.Items.Select(i => i.ProductId).ToList();
        var products = await productRepository.GetByIdsAsync(productIds);

        var orderCreationResult = OrderFactory.CreateOrderWithItems(
            Guid.NewGuid(),
            command.CustomerId,
            command.Items,
            products
        );

        if (!orderCreationResult.IsSuccess)
        {
            return Result<Guid>.Failure(
                orderCreationResult.ErrorCode!,
                orderCreationResult.Message!
            );
        }

        var order = orderCreationResult.Value!;
        await orderRepository.AddAsync(order);

        return Result<Guid>.Success(order.Id);
    }
}
