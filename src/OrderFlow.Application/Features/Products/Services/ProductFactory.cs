using OrderFlow.Application.Common.Models;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Products.Services;

public class ProductFactory
{
    public static Result<Product> CreateProduct(
        string sku,
        string name,
        string description,
        decimal price,
        int stock = 0)
    {
        var product = Product.Create(sku, name, description, price, stock);
        return Result<Product>.Success(product);
    }
}
