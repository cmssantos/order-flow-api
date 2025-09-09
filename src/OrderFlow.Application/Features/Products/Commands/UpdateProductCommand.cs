namespace OrderFlow.Application.Features.Products.Commands;

public record UpdateProductCommand(Guid Id, string Name, string Sku, decimal Price);
