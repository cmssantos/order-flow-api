using OrderFlow.Domain.Common;
using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities;

public class Product: BaseEntity
{
    public Sku Sku { get; private set; } = null!;
    public ProductName Name { get; private set; } = null!;
    public UnitPrice Price { get; private set; } = null!;

    private Product()
    {
        // For EF Core
    }

    private Product(Sku sku, ProductName name, UnitPrice price)
    {
        Sku = sku;
        Name = name;
        Price = price;
    }

    public static Product Create(string sku, string name, decimal price)
    {
        var productSku = Sku.Create(sku);
        var productName = ProductName.Create(name);
        var productPrice = UnitPrice.Create(price);

        return new Product(productSku, productName, productPrice);
    }

    public void Update(string name, decimal price)
    {
        var updatedName = ProductName.Create(name);
        var updatedPrice = UnitPrice.Create(price);

        Name = updatedName;
        Price = updatedPrice;
    }
}
