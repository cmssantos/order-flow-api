namespace OrderFlow.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorCode { get; }
    public string? Message { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(string errorCode, string message)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        Message = message;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(string errorCode, string message) => new(errorCode, message);
}
