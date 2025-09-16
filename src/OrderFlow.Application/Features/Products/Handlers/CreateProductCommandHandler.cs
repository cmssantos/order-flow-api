using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Features.Products.Services;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class CreateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<CreateProductCommandHandler> logger)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<CreateProductCommandHandler> logger = logger;

    public async Task<Result<Guid>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to create product with SKU {Sku}", command.Sku);

        var existing = await unitOfWork.Products.GetBySkuAsync(command.Sku, cancellationToken);
        if (existing is not null)
        {
            logger.LogWarning("Product creation failed: SKU {Sku} is already in use", command.Sku);

            return Result<Guid>.Failure(
                "Product.SkuAlreadyUsed",
                $"SKU '{command.Sku}' is already in use."
            );
        }

        var productCreationResult = ProductFactory.CreateProduct(
            command.Sku,
            command.Name,
            command.Description,
            command.Price,
            command.Stock
        );

        var product = productCreationResult.Value!;
        await unitOfWork.Products.AddAsync(product, cancellationToken);

        logger.LogInformation(
            "Saving product {ProductId} with SKU {Sku} to database",
            product.Id,
            product.Sku.Value
        );
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Product {ProductId} created successfully with SKU {Sku}",
            product.Id,
            product.Sku.Value
        );

        return Result<Guid>.Success(product.Id);
    }
}
