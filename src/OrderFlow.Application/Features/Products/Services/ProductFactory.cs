using OrderFlow.Application.Common.Models;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Products.Services;

public class ProductFactory
{
    public static Result<Product> CreateProduct(
        Guid productId,
        string sku,
        string name,
        decimal price)
    {
        var product = Product.Create(productId, sku, name, price);
        return Result<Product>.Success(product);
    }
}
