namespace OrderFlow.Application.Interfaces.Logging;

public interface IAppLogger<T>
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(Exception ex, string message);

    void LogInformation(string messageTemplate, params object[] args);
    void LogWarning(string messageTemplate, params object[] args);
    void LogError(Exception ex, string messageTemplate, params object[] args);
}
