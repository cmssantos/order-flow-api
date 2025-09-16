using OrderFlow.Domain.Common;
using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities;

public class Product: BaseEntity
{
    public Sku Sku { get; private set; } = null!;
    public ProductName Name { get; private set; } = null!;
    public ProductDescription Description { get; private set; } = null!;
    public UnitPrice Price { get; private set; } = null!;
    public StockQuantity Stock { get; private set; } = null!;

    private Product()
    {
        // For EF Core
    }

    private Product(
        Sku sku,
        ProductName name,
        ProductDescription description,
        UnitPrice price,
        StockQuantity stock)
    {
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;

        SetCreated();
    }

    public static Product Create(
        string sku,
        string name,
        string description,
        decimal price,
        int stock = 0)
    {
        var productSku = Sku.Create(sku);
        var productName = ProductName.Create(name);
        var productDescription = ProductDescription.Create(description);
        var productPrice = UnitPrice.Create(price);
        var productStock = StockQuantity.Create(stock);

        return new Product(productSku, productName, productDescription, productPrice, productStock);
    }

    public void Update(string name, string description)
    {
        var updatedName = ProductName.Create(name);
        var productDescription = ProductDescription.Create(description);

        if (Name != updatedName || Description != productDescription)
        {
            Name = updatedName;
            Description = productDescription;
            SetUpdated();
        }
    }

    public void UpdatePrice(decimal newPrice)
    {
        var updatedPrice = UnitPrice.Create(newPrice);
        if (Price.Value != newPrice)
        {
            Price = updatedPrice;
            SetUpdated();
        }
    }

    public void AdjustStock(int adjustment)
    {
        var newStock = Stock.Adjust(adjustment);
        if (newStock.Value != Stock.Value)
        {
            Stock = newStock;
            SetUpdated();
        }
    }
}
