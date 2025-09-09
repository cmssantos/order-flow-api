using OrderFlow.Application;
using OrderFlow.Api.Endpoints;
using OrderFlow.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
  app.MapScalarApiReference(options =>
  {
    options.Title = "OrderFlow API";
    options.Theme = ScalarTheme.DeepSpace; // Light, Dark, Solarized, etc.
  });
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
