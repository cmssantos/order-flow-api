using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class UpdateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<UpdateProductCommandHandler> logger)
    : IRequestHandler<UpdateProductCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<UpdateProductCommandHandler> logger = logger;

    public async Task<Result<Unit>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to update product {ProductId}", command.Id);

        var product = await unitOfWork.Products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found for update", command.Id);

            return Result<Unit>.Failure(
                "Product.NotFound",
                $"Product with ID {command.Id} not found."
            );
        }

        product.Update(command.Name, command.Description);
        unitOfWork.Products.Update(product);

        logger.LogInformation("Saving updates for product {ProductId}", product.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Product {ProductId} updated successfully", product.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
