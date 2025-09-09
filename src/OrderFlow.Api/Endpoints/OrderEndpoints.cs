using MediatR;
using OrderFlow.Application.Features.Orders.Commands;
using OrderFlow.Application.Features.Orders.Queries;
using OrderFlow.Application.DTOs;

namespace OrderFlow.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapPost("/", async (CreateOrderCommand command, ISender sender) =>
        {
            var orderId = await sender.Send(command);
            return Results.Created($"/api/orders/{orderId}", new { OrderId = orderId });
        })
        .Produces<object>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("CreateOrder");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetOrderByIdQuery(id);
            var order = await sender.Send(query);

            return order is not null ? Results.Ok(order) : Results.NotFound();
        })
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetOrderById");
    }
}
