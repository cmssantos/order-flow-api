using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Products.Queries;

namespace OrderFlow.Application.Features.Products.Handlers;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(query.Id);
        if (product == null)
        {
            return Result<ProductDto>.Failure(
                "Product.NotFound",
                $"Product with ID {query.Id} not found."
            );
        }

        var productDto = MapToDto(product);

        return Result<ProductDto>.Success(productDto);
    }

    private static ProductDto MapToDto(Domain.Entities.Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name.Value,
            Sku = product.Sku.Value,
            Price = product.Price.Value,
        };
    }
}
