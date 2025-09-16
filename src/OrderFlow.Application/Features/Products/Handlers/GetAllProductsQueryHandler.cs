using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Domain.Interfaces.Repositories;
using OrderFlow.Application.Features.Products.Queries;
using OrderFlow.Application.Interfaces.Logging;

namespace OrderFlow.Application.Features.Products.Handlers;

public class GetAllProductsQueryHandler(
    IUnitOfWork unitOfWork,
    IAppLogger<GetAllProductsQueryHandler> logger)
    : IRequestHandler<GetAllProductsQuery, Result<List<ProductListDto>>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IAppLogger<GetAllProductsQueryHandler> logger = logger;

    public async Task<Result<List<ProductListDto>>> Handle(
        GetAllProductsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to retrieve all products");

        var products = await unitOfWork.Products.GetAllAsync(cancellationToken);

        var productListDto = new List<ProductListDto>();
        foreach (var product in products)
        {
            productListDto.Add(new ProductListDto
            {
                Id = product.Id,
                Name = product.Name.Value,
                Sku = product.Sku.Value,
                Price = product.Price.Value
            });
        }

        logger.LogInformation("Retrieved {ProductCount} products", productListDto.Count);

        return Result<List<ProductListDto>>.Success(productListDto);
    }
}
