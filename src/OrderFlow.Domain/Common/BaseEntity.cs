namespace OrderFlow.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void SetCreated() => CreatedAt = DateTime.UtcNow;

    public void SetUpdated() => UpdatedAt = DateTime.UtcNow;
}
