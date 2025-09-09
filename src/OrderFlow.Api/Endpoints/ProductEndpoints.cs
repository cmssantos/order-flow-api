using MediatR;
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

        group.MapGet("/", async (ISender sender) =>
        {
            var products = await sender.Send(new GetAllProductsQuery());
            return Results.Ok(products ?? new List<ProductDto>());
        })
        .Produces<List<ProductDto>>(StatusCodes.Status200OK)
        .WithName("GetAllProducts");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var product = await sender.Send(new GetProductByIdQuery(id));
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetProductById");

        group.MapPost("/", async (CreateProductRequest request, ISender sender) =>
        {
            var command = new CreateProductCommand(request.Name, request.Sku, request.Price);
            var productId = await sender.Send(command);
            return Results.Created($"/api/products/{productId}", new { ProductId = productId });
        })
        .Produces<object>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("CreateProduct");

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, ISender sender) =>
        {
            var command = new UpdateProductCommand(id, request.Name, request.Sku, request.Price);
            await sender.Send(command);
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("UpdateProduct");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteProductCommand(id));
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteProduct");
    }
}
