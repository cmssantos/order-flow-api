using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Features.Products.Services;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Products.GetBySkuAsync(command.Sku, cancellationToken);
        if (existing is not null)
        {
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
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}
