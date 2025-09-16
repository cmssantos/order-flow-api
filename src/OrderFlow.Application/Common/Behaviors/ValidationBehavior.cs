using FluentValidation;
using MediatR;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!validators.Any())
        {
            return await next(ct);
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, ct))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next(ct);
        }

        var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));

        var genericType = typeof(TResponse);
        if (genericType.IsGenericType && genericType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = genericType.GetGenericArguments()[0];
            var failureMethod = genericType
                .GetMethod(nameof(Result<object>.Failure))!;

            var failure = failureMethod.Invoke(null, ["ValidationError", errorMessage]);
            return (TResponse)failure!;
        }

        return await next(ct);
    }
}
