using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Interfaces.Logging;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Application.Common.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse>(IAppLogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        try
        {
            return await next(ct);
        }
        catch (DomainValidationException dex)
        {
            logger.LogWarning(
                "Domain validation failed for request {RequestType}: {ErrorMessage}",
                typeof(TRequest).Name,
                dex.Message
            );

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failureMethod = typeof(TResponse)
                    .GetMethod(nameof(Result<object>.Failure))!;
                var failure = failureMethod.Invoke(null, ["ValidationError", dex.Message]);

                return (TResponse)failure!;
            }

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unhandled exception for request {RequestType}",
                typeof(TRequest).Name
            );

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failureMethod = typeof(TResponse)
                    .GetMethod(nameof(Result<object>.Failure))!;

                var failure = failureMethod.Invoke(
                    null,
                    ["InternalError", "An unexpected error occurred."]
                );

                return (TResponse)failure!;
            }

            throw;
        }
    }
}
