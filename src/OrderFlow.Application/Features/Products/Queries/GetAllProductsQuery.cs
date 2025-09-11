using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Features.Products.Queries;

public record GetAllProductsQuery: IRequest<Result<List<ProductDto>>>;
