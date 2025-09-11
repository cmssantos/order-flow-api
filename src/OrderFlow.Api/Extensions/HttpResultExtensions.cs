using MediatR;
using OrderFlow.Api.Common.Models;
using OrderFlow.Application.Common.Models;

namespace OrderFlow.Api.Extensions;

public static class HttpResultExtensions
{
    private static IResult SelectResultByErrorCode(ApiResponse<object> response)
    {
        var errorCode = response.ErrorCode?.ToLower() ?? string.Empty;

        if (errorCode.Contains("notFound", StringComparison.CurrentCultureIgnoreCase))
        {
            return Results.NotFound(response);
        }

        if (errorCode.Contains("alreadyUsed", StringComparison.CurrentCultureIgnoreCase) ||
            errorCode.Contains("alreadyExists", StringComparison.CurrentCultureIgnoreCase) ||
            errorCode.Contains("conflict"))
        {
            return Results.Conflict(response);
        }

        return Results.BadRequest(response);
    }

    public static IResult ToHttpResult<T>(this Result<T> result, string? locationUri = null)
    {
        if (result.IsSuccess)
        {
            if (locationUri != null)
            {
                return Results.Created(locationUri, new ApiResponse<T>(true, result.Value, null, null));
            }

            if (typeof(T) == typeof(Unit) || typeof(T) == typeof(void))
            {
                return Results.NoContent();
            }

            return Results.Ok(new ApiResponse<T>(true, result.Value, null, null));
        }

        var response = new ApiResponse<object>(false, null, result.ErrorCode, result.Message);
        return SelectResultByErrorCode(response);
    }
}
