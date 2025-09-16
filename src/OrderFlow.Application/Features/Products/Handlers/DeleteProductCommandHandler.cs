using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class DeleteProductCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<DeleteProductCommandHandler> logger)
    : IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<DeleteProductCommandHandler> logger = logger;

    public async Task<Result<Unit>> Handle(
        DeleteProductCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete product {ProductId}", command.Id);

        var product = await unitOfWork.Products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found for deletion", command.Id);

            return Result<Unit>.Failure(
                "Product.NotFound",
                $"Product with ID {command.Id} not found."
            );
        }

        await unitOfWork.Products.DeleteAsync(product.Id, cancellationToken);

        logger.LogInformation("Saving deletion of product {ProductId}", product.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Product {ProductId} deleted successfully", product.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
