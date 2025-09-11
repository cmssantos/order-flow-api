using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Products.Queries;

namespace OrderFlow.Application.Features.Products.Handlers;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<Result<List<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync();

        var productsDto = new List<ProductDto>();
        foreach (var product in products)
        {
            productsDto.Add(new ProductDto
            {
                Id = product.Id,
                Name = product.Name.Value,
                Sku = product.Sku.Value,
                Price = product.Price.Value
            });
        }

        return Result<List<ProductDto>>.Success(productsDto);
    }
}
