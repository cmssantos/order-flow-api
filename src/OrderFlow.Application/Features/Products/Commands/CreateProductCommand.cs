using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Commands;

public record CreateProductCommand(string Name, string Sku, decimal Price ) : IRequest<Result<Guid>>;
