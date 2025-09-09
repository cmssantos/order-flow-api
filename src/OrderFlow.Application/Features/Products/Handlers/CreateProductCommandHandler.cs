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
    
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await productRepository.GetBySkuAsync(request.Sku);
        if (existing is not null)
        {
            return Result<Guid>.Failure($"A product with SKU '{request.Sku}' already exists.");
        }

        var productCreationResult = ProductFactory
            .CreateProduct(Guid.NewGuid(), request.Name, request.Sku, request.Price);

        if (!productCreationResult.IsSuccess)
        {
            return Result<Guid>
                .Failure(productCreationResult.Error ?? "An unexpected error occurred.");
        }

        var product = productCreationResult.Value!;
        await productRepository.AddAsync(product);

        return Result<Guid>.Success(product.Id);
    }
}
