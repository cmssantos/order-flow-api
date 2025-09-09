namespace OrderFlow.Api.Endpoints;

public static class EndpointExtensions
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app.MapGroup("/api");

        apiGroup.MapGet("/health", () => Results.Ok("API is healthy."))
            .WithTags("Health");

        apiGroup.MapProductEndpoints();
        apiGroup.MapCustomerEndpoints();
        apiGroup.MapOrderEndpoints();
    }
}
