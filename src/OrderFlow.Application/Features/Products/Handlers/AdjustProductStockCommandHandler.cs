using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class AdjustProductStockCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<AdjustProductStockCommandHandler> logger)
    : IRequestHandler<AdjustProductStockCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<AdjustProductStockCommandHandler> logger = logger;

    public async Task<Result<Unit>> Handle(
        AdjustProductStockCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Attempting to adjust stock for product {ProductId} by {Adjustment}",
            command.Id,
            command.Adjustment
        );

        var product = await unitOfWork.Products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found for stock adjustment", command.Id);

            return Result<Unit>.Failure(
                "Product.NotFound",
                $"Product with ID {command.Id} not found."
            );
        }

        product.AdjustStock(command.Adjustment);
        unitOfWork.Products.Update(product);

        logger.LogInformation("Saving stock adjustment for product {ProductId}", product.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Product {ProductId} stock adjusted successfully", product.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
