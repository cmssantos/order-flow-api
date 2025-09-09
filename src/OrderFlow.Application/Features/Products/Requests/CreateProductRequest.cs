namespace OrderFlow.Application.Features.Products.Requests;

public record CreateProductRequest(string Name, string Sku, decimal Price);
