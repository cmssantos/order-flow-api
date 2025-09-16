using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Features.Products.Requests;

namespace OrderFlow.Application.Features.Products.Mappers;

public static class ProductMapper
{
    public static CreateProductCommand ToCommand(this CreateProductRequest request)
        => new(
            request.Name,
            request.Sku,
            request.Description,
            request.Price,
            request.Stock
        );

    public static UpdateProductCommand ToCommand(this UpdateProductRequest request, Guid id)
        => new(
            id,
            request.Name,
            request.Description
        );

    public static UpdateProductPriceCommand ToCommand(this UpdateProductPriceRequest request, Guid id)
        => new(
            id,
            request.Price
        );

    public static AdjustProductStockCommand ToCommand(this AdjustProductStockRequest request, Guid id)
        => new(
            id,
            request.Adjustment
        );
}
