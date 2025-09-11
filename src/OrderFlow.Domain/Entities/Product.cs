using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public Sku Sku { get; private set; }
    public ProductName Name { get; private set; }
    public UnitPrice Price { get; private set; }

    private Product(Guid id, Sku sku, ProductName name, UnitPrice price)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Price = price;
    }

    public static Product Create(Guid id, string sku, string name, decimal price)
    {
        var productSku = Sku.Create(sku);
        var productName = ProductName.Create(name);
        var productPrice = UnitPrice.Create(price);

        return new Product(id, productSku, productName, productPrice);
    }

    public void Update(string name, decimal price)
    {
        var updatedName = ProductName.Create(name);
        var updatedPrice = UnitPrice.Create(price);

        Name = updatedName;
        Price = updatedPrice;
    }
}
