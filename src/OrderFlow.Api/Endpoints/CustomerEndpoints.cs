using MediatR;
using OrderFlow.Api.Common.Models;
using OrderFlow.Api.Extensions;
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
            var command = new GetAllCustomersQuery();

            var result = await sender.Send(command);
            return Results.Ok(result.Value);
        })
        .Produces<ApiResponse<List<CustomerDto>>>(StatusCodes.Status200OK)
        .WithName("GetAllCustomers");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new GetCustomerByIdQuery(id);

            var result = await sender.Send(command);
            return result.ToHttpResult();
        })
        .Produces<ApiResponse<CustomerDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("GetCustomerById");

        group.MapPost("/", async (CreateCustomerRequest request, ISender sender) =>
        {
            var command = new CreateCustomerCommand(request.FullName, request.Email);

            var result = await sender.Send(command);
            return result.ToHttpResult(locationUri: $"/api/customers/{result.Value}");
        })
        .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status409Conflict)
        .WithName("CreateCustomer");

        group.MapPut("/{id:guid}", async (Guid id, UpdateCustomerRequest request, ISender sender) =>
        {
            var command = new UpdateCustomerCommand(id, request.FullName, request.Email);

            var result = await sender.Send(command);
            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("UpdateCustomer");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteCustomerCommand(id);

            var result = await sender.Send(command);
            return result.ToHttpResult();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound)
        .WithName("DeleteCustomer");
    }
}
