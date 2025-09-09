namespace OrderFlow.Application.Features.Products.Requests;

public record UpdateProductRequest(string Name, string Sku, decimal Price);
