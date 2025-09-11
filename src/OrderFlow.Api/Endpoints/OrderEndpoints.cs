using MediatR;
using OrderFlow.Api.Common.Models;
using OrderFlow.Api.Extensions;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Application.Features.Orders.Queries;

namespace OrderFlow.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapPost("/", async (CreateOrderCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToHttpResult(locationUri: $"/api/orders/{result.Value}");
        })
        .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("CreateOrder");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetOrderByIdQuery(id);

            var result = await sender.Send(query);
            return result.ToHttpResult();
        })
        .Produces<ApiResponse<OrderDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("GetOrderById");
    }
}
