using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Queries;

public record GetProductBySkuQuery(string Sku): IRequest<Result<ProductDetailDto>>;
