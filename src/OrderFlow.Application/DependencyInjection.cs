using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Common.Behaviors;
using OrderFlow.Application.Features.Products.Commands;
using FluentValidation;

namespace OrderFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
