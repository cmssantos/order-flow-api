using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Products.Queries;

namespace OrderFlow.Application.Features.Products.Handlers;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<List<ProductDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.Products.GetAllAsync(cancellationToken);

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
