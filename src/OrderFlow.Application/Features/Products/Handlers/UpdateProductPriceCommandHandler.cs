using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class UpdateProductPriceCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateProductPriceCommandHandler> logger)
    : IRequestHandler<UpdateProductPriceCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<UpdateProductPriceCommandHandler> logger = logger;

    public async Task<Result<Unit>> Handle(
        UpdateProductPriceCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to update price for product {ProductId}", command.Id);

        var product = await unitOfWork.Products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found for price update", command.Id);

            return Result<Unit>.Failure(
                "Product.NotFound",
                $"Product with ID {command.Id} not found."
            );
        }

        product.UpdatePrice(command.Price);
        unitOfWork.Products.Update(product);

        logger.LogInformation("Saving price update for product {ProductId}", product.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Product {ProductId} price updated successfully", product.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
