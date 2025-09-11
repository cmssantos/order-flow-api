using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Commands;

public record UpdateProductCommand(Guid Id, string Name, decimal Price): IRequest<Result<Unit>>;
