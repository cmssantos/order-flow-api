namespace OrderFlow.Domain.Audit;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Insert, Update, Delete
    public string KeyValues { get; set; } = string.Empty; // { "Id": "..." }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? UserId { get; set; }
    public string? IpAddress { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
