namespace OrderFlow.Domain.Exceptions;

public class DomainValidationException(string errorCode, string defaultMessage, params object[] parameters)
    : Exception(defaultMessage)
{
    public string ErrorCode { get; } = errorCode;
    public object[] Parameters { get; } = parameters;
    public string DefaultMessage { get; } = defaultMessage;
}
