using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id): IRequest<Result<ProductDto>>;
