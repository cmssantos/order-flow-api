namespace OrderFlow.Api.Common.Models;

public record ApiResponse<T>(bool Success, T? Data, string? ErrorCode, string? Message);
