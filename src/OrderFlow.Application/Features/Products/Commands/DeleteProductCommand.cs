using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id): IRequest<Result<Unit>>;
