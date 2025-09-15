using OrderFlow.Application;
using OrderFlow.Api.Endpoints;
using OrderFlow.Infrastructure;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrderFlow API",
        Version = "v1"
    });
});

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference("/scalar", options =>
    {
        options.Title = "OrderFlow API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseSwagger(c =>
{
    c.RouteTemplate = "openapi/{documentName}/openapi.json"; // URL: /openapi/v1/openapi.json
});

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
