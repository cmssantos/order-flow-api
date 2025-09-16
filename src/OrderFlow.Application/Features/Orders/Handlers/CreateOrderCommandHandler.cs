using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Application.Features.Orders.Services;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Orders.Handlers;

public class CreateOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<CreateOrderCommandHandler> logger): IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<CreateOrderCommandHandler> logger = logger;

    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to create order for customer {CustomerId}", command.CustomerId);

        var productIds = command.Items.Select(i => i.ProductId).ToList();
        var products = await unitOfWork.Products.GetByIdsAsync(productIds, cancellationToken);

        var orderCreationResult = OrderFactory.CreateOrderWithItems(
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
        await unitOfWork.Orders.AddAsync(order, cancellationToken);

        logger.LogInformation(
            "Saving order {OrderId} for customer {CustomerId} to database",
            order.Id,
            command.CustomerId
        );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Order {OrderId} created successfully for customer {CustomerId}",
            order.Id,
            command.CustomerId
        );

        return Result<Guid>.Success(order.Id);
    }
}
