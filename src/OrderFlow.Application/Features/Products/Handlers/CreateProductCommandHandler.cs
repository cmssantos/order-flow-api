using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Features.Products.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class CreateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var existing = await productRepository.GetBySkuAsync(command.Sku, cancellationToken);
        if (existing is not null)
        {
            return Result<Guid>.Failure(
                "Product.SkuAlreadyUsed",
                $"SKU '{command.Sku}' is already in use."
            );
        }

        var productCreationResult = ProductFactory.CreateProduct(
            Guid.NewGuid(),
            command.Name,
            command.Sku,
            command.Price
        );

        var product = productCreationResult.Value!;
        await productRepository.AddAsync(product, cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}
