namespace OrderFlow.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected void SetCreated()
    {
        if (CreatedAt == default)
        {
            CreatedAt = DateTime.UtcNow;
        }
        UpdatedAt = CreatedAt;
    }

    protected void SetUpdated() => UpdatedAt = DateTime.UtcNow;
}
