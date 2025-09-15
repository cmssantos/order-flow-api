using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int Stock = 0)
    : IRequest<Result<Guid>>;
