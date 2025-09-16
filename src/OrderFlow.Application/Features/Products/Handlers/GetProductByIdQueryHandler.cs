using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Products.Queries;
using OrderFlow.Application.Interfaces.Logging;

namespace OrderFlow.Application.Features.Products.Handlers;

public class GetProductByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<GetProductByIdQueryHandler> logger)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDetailDto>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<GetProductByIdQueryHandler> logger = logger;

    public async Task<Result<ProductDetailDto>> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to retrieve product {ProductId}", query.Id);

        var product = await unitOfWork.Products.GetByIdAsync(query.Id, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product {ProductId} not found", query.Id);

            return Result<ProductDetailDto>.Failure(
                "Product.NotFound",
                $"Product with ID {query.Id} not found."
            );
        }

        var productDto = MapToDto(product);

        logger.LogInformation("Product {ProductId} retrieved successfully", query.Id);

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
