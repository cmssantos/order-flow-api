using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Commands;

public record UpdateProductPriceCommand(Guid Id, decimal Price): IRequest<Result<Unit>>;
