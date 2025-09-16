using MediatR;
using OrderFlow.Api.Common.Models;
using OrderFlow.Api.Extensions;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Features.Products.Queries;
using OrderFlow.Application.Features.Products.Requests;
using OrderFlow.Application.Features.Products.Mappers;

namespace OrderFlow.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var command = new GetAllProductsQuery();
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces<ApiResponse<List<ProductListDto>>>(StatusCodes.Status200OK)
        .WithName("GetAllProducts");

        group.MapGet("/{id:guid}",
            async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var command = new GetProductByIdQuery(id);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces<ApiResponse<ProductDetailDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("GetProductById");

        group.MapGet("/by-sku/{sku}",
            async (string sku, ISender sender, CancellationToken ct) =>
        {
            var command = new GetProductBySkuQuery(sku);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces<ApiResponse<ProductDetailDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("GetProductBySku");

        group.MapPost("/",
            async (CreateProductRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.ToCommand();
            var result = await sender.Send(command, ct);

            var locationUri = $"{group}/{result.Value}";
            return result.ToHttpResult(locationUri);
        })
        .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status409Conflict)
        .WithName("CreateProduct");

        group.MapPut("/{id:guid}",
            async (Guid id, UpdateProductRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.ToCommand(id);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("UpdateProduct");

        group.MapPatch("/{id:guid}/price",
            async (Guid id, UpdateProductPriceRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.ToCommand(id);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("UpdateProductPrice");

        group.MapPatch("/{id:guid}/stock",
             async (Guid id, AdjustProductStockRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = request.ToCommand(id);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("AdjustProductStock");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var command = new DeleteProductCommand(id);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("DeleteProduct");
    }
}
