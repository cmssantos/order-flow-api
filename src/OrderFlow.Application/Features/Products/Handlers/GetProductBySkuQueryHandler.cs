using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Products.Queries;
using OrderFlow.Application.Interfaces.Logging;

namespace OrderFlow.Application.Features.Products.Handlers;

public class GetProductBySkuQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<GetProductBySkuQueryHandler> logger)
    : IRequestHandler<GetProductBySkuQuery, Result<ProductDetailDto>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<GetProductBySkuQueryHandler> logger = logger;

    public async Task<Result<ProductDetailDto>> Handle(
        GetProductBySkuQuery query,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetBySkuAsync(query.Sku, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with SKU {Sku} not found", query.Sku);

            return Result<ProductDetailDto>.Failure(
                "Product.NotFound",
                $"Product with SKU {query.Sku} not found."
            );
        }

        var productDto = MapToDto(product);

        logger.LogInformation("Product with SKU {Sku} retrieved successfully", query.Sku);

        return Result<ProductDetailDto>.Success(productDto);
    }

    private static ProductDetailDto MapToDto(Domain.Entities.Product product)
    {
        return new ProductDetailDto
        {
            Id = product.Id,
            Name = product.Name.Value,
            Description = product.Description.Value,
            Sku = product.Sku.Value,
            Price = product.Price.Value,
            Stock = product.Stock.Value,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
