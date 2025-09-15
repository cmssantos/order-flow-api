namespace OrderFlow.Application.Features.Products.Requests;

public record CreateProductRequest(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int Stock = 0);
