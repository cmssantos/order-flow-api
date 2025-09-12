using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Application.Features.Orders.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Orders.Handlers;

public class CreateOrderCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
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
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(order.Id);
    }
}
