using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Commands;

public record AdjustProductStockCommand(Guid Id, int Adjustment) : IRequest<Result<Unit>>;
