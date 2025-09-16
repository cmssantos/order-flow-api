using Microsoft.Extensions.Logging;
using OrderFlow.Application.Interfaces.Logging;

namespace OrderFlow.Infrastructure.Logging;

public class SerilogLogger<T>(ILogger<T> logger): IAppLogger<T>
{
    private readonly ILogger<T> logger = logger;

    public void LogInformation(string message)
        => logger.LogInformation("{Message}", message);

    public void LogWarning(string message)
        => logger.LogWarning("{Message}", message);

    public void LogError(Exception ex, string message)
        => logger.LogError(ex, "{Message}", message);

#pragma warning disable CA2254
    public void LogInformation(string messageTemplate, params object[] args)
        => logger.LogInformation(messageTemplate, args);

    public void LogWarning(string messageTemplate, params object[] args)
        => logger.LogWarning(messageTemplate, args);

    public void LogError(Exception ex, string messageTemplate, params object[] args)
        => logger.LogError(ex, messageTemplate, args);
#pragma warning restore CA2254
}
