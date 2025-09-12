using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Audit;
using System.Text.Json;

namespace OrderFlow.Infrastructure.Interceptors;

public class AuditInterceptor(IUserContext userContext): SaveChangesInterceptor
{
    private readonly IUserContext userContext = userContext;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            var auditLogs = CreateAuditLogs(eventData.Context);
            if (auditLogs.Count > 0)
            {
                eventData.Context.Set<AuditLog>().AddRange(auditLogs);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private List<AuditLog> CreateAuditLogs(DbContext context)
    {
        var logs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            var log = new AuditLog
            {
                TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                UserId = userContext.UserId,
                IpAddress = userContext.IpAddress,
                Timestamp = DateTime.UtcNow
            };

            // Primary key values
            var keyValues = entry.Properties
                .Where(p => p.Metadata.IsPrimaryKey())
                .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
            log.KeyValues = JsonSerializer.Serialize(keyValues);

            // Old and new values
            if (entry.State == EntityState.Modified)
            {
                log.OldValues = JsonSerializer.Serialize(
                    entry.Properties.Where(p => p.IsModified)
                                    .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));

                log.NewValues = JsonSerializer.Serialize(
                    entry.Properties.Where(p => p.IsModified)
                                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
            }
            else if (entry.State == EntityState.Added)
            {
                log.NewValues = JsonSerializer.Serialize(
                    entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
            }
            else if (entry.State == EntityState.Deleted)
            {
                log.OldValues = JsonSerializer.Serialize(
                    entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
            }

            logs.Add(log);
        }

        return logs;
    }
}
