using MediatR;
using OrderFlow.Application.Common.DTOs;
using OrderFlow.Application.Features.Customers.Commands;
using OrderFlow.Application.Features.Customers.Queries;
using OrderFlow.Application.Features.Customers.Requests;

namespace OrderFlow.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers").WithTags("Customers");

        group.MapGet("/", async (ISender sender) =>
        {
            var customers = await sender.Send(new GetAllCustomersQuery());
            return Results.Ok(customers ?? new List<CustomerDto>());
        })
        .Produces<List<CustomerDto>>(StatusCodes.Status200OK)
        .WithName("GetAllCustomers");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var customer = await sender.Send(new GetCustomerByIdQuery(id));
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        })
        .Produces<CustomerDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetCustomerById");

        group.MapPost("/", async (CreateCustomerRequest request, ISender sender) =>
        {
            var command = new CreateCustomerCommand(request.FullName, request.Email);
            var customerId = await sender.Send(command);
            return Results.Created($"/api/customers/{customerId}", new { CustomerId = customerId });
        })
        .Produces<object>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("CreateCustomer");

        group.MapPut("/{id:guid}", async (Guid id, UpdateCustomerRequest request, ISender sender) =>
        {
            var command = new UpdateCustomerCommand(id, request.FullName, request.Email);
            await sender.Send(command);
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("UpdateCustomer");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteCustomerCommand(id));
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteCustomer");
    }
}
