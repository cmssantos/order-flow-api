using MediatR;
using OrderFlow.Api.Common.Models;
using OrderFlow.Api.Extensions;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Application.Features.Products.Queries;
using OrderFlow.Application.Features.Products.Requests;

namespace OrderFlow.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetAllProductsQuery();

            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result.Value);
        })
        .Produces<ApiResponse<List<ProductDto>>>(StatusCodes.Status200OK)
        .WithName("GetAllProducts");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetProductByIdQuery(id);

            var result = await sender.Send(command, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("GetProductById");

        group.MapPost("/", async (CreateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateProductCommand(request.Name, request.Sku, request.Price);

            var result = await sender.Send(command, cancellationToken);
            return result.ToHttpResult(locationUri: $"/api/products/{result.Value}");
        })
        .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status409Conflict)
        .WithName("CreateProduct");

        group.MapPut("/{id:guid}",
            async (Guid id, UpdateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateProductCommand(id, request.Name, request.Price);

            var result = await sender.Send(command, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("UpdateProduct");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteProductCommand(id);

            var result = await sender.Send(command, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("DeleteProduct");
    }
}
